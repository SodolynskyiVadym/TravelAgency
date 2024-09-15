using TravelAgencyService.Services;
using TravelAgencyService.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
builder.Services.Configure<RabbitMqSetting>(builder.Configuration.GetSection("RabbitMqSetting"));

// Register the consumer service as a hosted service only
builder.Services.AddHostedService<RabbitConsumer>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();