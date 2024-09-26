using Microsoft.Extensions.Options;
using TravelAgencyService.Services;
using TravelAgencyService.Settings;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<RabbitMqSetting>(builder.Configuration.GetSection("RabbitMqSetting"));
}
else if(builder.Environment.IsEnvironment("DockerEnv"))
{
    var rabbitMqSetting = new RabbitMqSetting
    {
        Host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? throw new InvalidOperationException(),
        Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? throw new InvalidOperationException()),
        UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? throw new InvalidOperationException(),
        Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? throw new InvalidOperationException()
    };
    builder.Services.AddSingleton(Options.Create(rabbitMqSetting));
}
else{
    builder.Services.Configure<RabbitMqSetting>(builder.Configuration.GetSection("RabbitMqSetting"));
}

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));

builder.Services.AddHostedService<RabbitConsumer>();


var app = builder.Build();


app.Run();