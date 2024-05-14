using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("secret.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString:DefaultConnection"]);
    Console.WriteLine("Connected to database");
});

builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("DevCors");
app.MapControllers();

app.Run();