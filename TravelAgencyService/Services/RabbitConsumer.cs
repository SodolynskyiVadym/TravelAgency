﻿using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TravelAgencyService.Models;
using TravelAgencyService.Settings;

namespace TravelAgencyService.Services;

public class RabbitConsumer : BackgroundService
{
    private readonly RabbitMqSetting _rabbitMqSetting;
    private readonly MailService _mailService;
    private IConnection _connection;
    private IModel _channel;

    public RabbitConsumer(IOptions<RabbitMqSetting> rabbitMqSetting, IOptions<MailSetting> mailSetting)
    {
        _rabbitMqSetting = rabbitMqSetting.Value;
        _mailService = new MailService(mailSetting.Value);

        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSetting.Host,
            UserName = _rabbitMqSetting.UserName,
            Password = _rabbitMqSetting.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartConsuming(_rabbitMqSetting.QueueName, stoppingToken);
        await Task.CompletedTask;
    }

    private void StartConsuming(string queueName, CancellationToken cancellationToken)
    {
        _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Tour tour = JsonConvert.DeserializeObject<Tour>(message);
            Console.WriteLine($"Received message: {tour}");
            Console.WriteLine(_mailService.SendTourMessage("test@gmail.com", tour));
        };
        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}