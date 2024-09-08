using TravelAgencyService.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();