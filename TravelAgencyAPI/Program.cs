using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Stripe;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Repositories;
using TravelAgencyAPI.Settings;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<AuthSetting>(builder.Configuration.GetSection("AuthSetting"));
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

string connectionString = builder.Configuration["ConnectionString:DefaultConnection"] ?? throw new InvalidOperationException();

builder.Services.AddDbContext<TravelDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

string redisConnectionString = builder.Configuration["ConnectionString:RedisConnection"] ?? throw new InvalidOperationException();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(redisConnectionString));


builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
    {
        SchemaName = "HangfireSchema",
        PrepareSchemaIfNecessary = true,
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

string tokenKeyString = builder.Configuration.GetSection("AuthSetting:TokenKey").Value ?? throw new InvalidOperationException();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

WebApplication app = builder.Build();

// app.UseMiddleware<ExceptionMiddlewareHandler>();
app.UseCors("DevCors");
app.UseHangfireDashboard();

var recurringJobManager = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IRecurringJobManager>();
recurringJobManager.AddOrUpdate<PaymentService>(
    recurringJobId: "DeleteUnpaidPaymentsJob", 
    methodCall: r => r.DeleteUnpaid(), 
    cronExpression: "59 23 * * *", 
    options: new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    });

recurringJobManager.AddOrUpdate<TourService>(
    recurringJobId: "CheckActiveToursJob", 
    methodCall: t => t.CheckTourAvailability(), 
    cronExpression: "01 00 * * *", 
    options: new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    });
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();