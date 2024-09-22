using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TravelAgencyService.Models;
using TravelAgencyService.Settings;

namespace TravelAgencyService.Services;

public class RabbitConsumer : BackgroundService
{
    private readonly MailService _mailService;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly QueueSetting _queueSetting = new QueueSetting() {AutoAck = true};

    public RabbitConsumer(IOptions<RabbitMqSetting> rabbitMqSettingOptions, IOptions<MailSetting> mailSetting)
    {
        var rabbitMqSetting = rabbitMqSettingOptions.Value;
        _mailService = new MailService(mailSetting.Value);

        var factory = new ConnectionFactory
        {
            HostName = rabbitMqSetting.Host,
            UserName = rabbitMqSetting.UserName,
            Password = rabbitMqSetting.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartEmailMessageConsuming<TourEmailDto>("payment-queue", _queueSetting, stoppingToken, tour =>
        {
            _mailService.SendTourMessage(tour.Email, tour);
        });
        StartEmailMessageConsuming<User>("create-user-queue", _queueSetting, stoppingToken, user =>
        {
            _mailService.SendPassword(user.Email, user);
        });
        StartEmailMessageConsuming<User>("reserve-password-queue", _queueSetting, stoppingToken, user =>
        {
            _mailService.SendReservePassword(user.Email, user.Password);
        });
        await Task.CompletedTask;
    }

    private void StartEmailMessageConsuming<T>(string queueName, QueueSetting queueSetting, CancellationToken stoppingToken, Action<T> action)
    {
        _channel.QueueDeclare(queue: queueName, durable: queueSetting.Durable, exclusive: queueSetting.Exclusive, autoDelete: queueSetting.AutoDelete);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var obj = JsonConvert.DeserializeObject<T>(message);
            if (obj != null) action(obj);
        };
        _channel.BasicConsume(queue: queueName, autoAck: queueSetting.AutoAck, consumer: consumer);
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}