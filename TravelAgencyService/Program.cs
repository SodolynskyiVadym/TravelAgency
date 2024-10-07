using Microsoft.Extensions.Options;
using TravelAgencyService.Services;
using TravelAgencyService.Settings;

var builder = WebApplication.CreateBuilder(args);

string rabbitConnectionString;

if (builder.Environment.IsDevelopment())
{
    rabbitConnectionString = builder.Configuration.GetConnectionString("RabbitMqConnection") ?? throw new InvalidOperationException();
} else if (builder.Environment.IsEnvironment("DockerEnv"))
{
    rabbitConnectionString = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING") ?? throw new InvalidOperationException();
} else if (builder.Environment.IsEnvironment("AzureEnv"))
{
    rabbitConnectionString = Environment.GetEnvironmentVariable("AzureRabbitMqConnection") ?? throw new InvalidOperationException();
} else 
{
    rabbitConnectionString = builder.Configuration.GetConnectionString("RabbitMqConnection") ?? throw new InvalidOperationException();
}

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));

builder.Services.AddHostedService(sp =>
{
    var mailSetting = sp.GetRequiredService<IOptions<MailSetting>>();
    return new RabbitConsumer(mailSetting, rabbitConnectionString);
});


var app = builder.Build();

app.Run();