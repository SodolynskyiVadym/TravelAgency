using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Stripe;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Services;
using TravelAgencyAPIServer.Services.Interfaces;
using TravelAgencyAPIServer.Settings;


var builder = WebApplication.CreateBuilder(args);
string connectionString;
string redisConnectionString;
string rabbitConnectionString;

if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException();
    redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection") ?? throw new InvalidOperationException();
    rabbitConnectionString = builder.Configuration.GetConnectionString("RabbitMqConnection") ?? throw new InvalidOperationException();
}
else if (builder.Environment.IsEnvironment("DockerEnv"))
{
    connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? throw new InvalidOperationException();
    redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ?? throw new InvalidOperationException();
    rabbitConnectionString = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING") ?? throw new InvalidOperationException();
}
else if(builder.Environment.IsEnvironment("AzureEnv"))
{
    connectionString = Environment.GetEnvironmentVariable("AzureConnection") ?? throw new InvalidOperationException();
    redisConnectionString = Environment.GetEnvironmentVariable("AzureRedisConnection") ?? throw new InvalidOperationException();
    rabbitConnectionString = Environment.GetEnvironmentVariable("AzureRabbitMqConnection") ?? throw new InvalidOperationException();
}
else
{
    connectionString = builder.Configuration.GetConnectionString("ConnectionStrings:ProductionConnection") ?? throw new InvalidOperationException();
    redisConnectionString = builder.Configuration.GetConnectionString("ConnectionStrings:ProductionRedisConnection") ?? throw new InvalidOperationException();
    rabbitConnectionString = builder.Configuration.GetConnectionString("ConnectionStrings:ProductionRabbitMqConnection") ?? throw new InvalidOperationException();
}

string tokenKeyString = builder.Configuration.GetSection("AuthSetting:TokenKey").Value ?? throw new InvalidOperationException();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<AuthSetting>(builder.Configuration.GetSection("AuthSetting"));
builder.Services.Configure<AddressSetting>(builder.Configuration.GetSection("Address"));
builder.Services.Configure<List<RabbitMqQueueSetting>>(builder.Configuration.GetSection("Queues"));

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.Services.AddDbContext<TravelDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});


builder.Services.AddSingleton<DapperDbContext>(sp => new DapperDbContext(connectionString));


builder.Services.AddSingleton<IRabbitMqPublisher>(sp =>
{
    var options = sp.GetRequiredService<IOptions<List<RabbitMqQueueSetting>>>();
    return new RabbitMqPublisher(options, rabbitConnectionString);
});


builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TravelDbContext>();
    context.Database.EnsureCreated();
}

app.UseHangfireDashboard();

var recurringJobManager = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IRecurringJobManager>();
recurringJobManager.AddOrUpdate<PaymentService>(
    recurringJobId: "DeleteUnpaidPaymentsJob", 
    methodCall: p => p.DeleteUnpaid(), 
    cronExpression: "59 23 * * *", 
    options: new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    });

recurringJobManager.AddOrUpdate<PaymentService>(
    recurringJobId: "DeleteOldPaymentsJob", 
    methodCall: p => p.DeleteOldPayments(), 
    cronExpression: "01 00 * * *", 
    options: new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    });

recurringJobManager.AddOrUpdate<TourService>(
    recurringJobId: "CheckActiveToursJob", 
    methodCall: p => p.CheckTourAvailability(), 
    cronExpression: "01 00 * * *", 
    options: new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    });

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();