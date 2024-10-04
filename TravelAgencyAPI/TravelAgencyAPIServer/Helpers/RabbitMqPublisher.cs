using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using TravelAgencyAPIServer.Services.Interfaces;
using TravelAgencyAPIServer.Settings;
using IModel = RabbitMQ.Client.IModel;

namespace TravelAgencyAPIServer.Helpers;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly RabbitMqSetting _rabbitMqSetting;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqPublisher(IOptions<RabbitMqSetting> rabbitMqSetting)
    {
        _rabbitMqSetting = rabbitMqSetting.Value;
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSetting.Host,
            UserName = _rabbitMqSetting.UserName,
            Password = _rabbitMqSetting.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        foreach (var queue in _rabbitMqSetting.Queues)
        {
            _channel.QueueDeclare(queue: queue.Name, durable: queue.Durable, exclusive: queue.Exclusive, autoDelete: queue.AutoDelete, arguments: null);
        }
    }
    
    public async Task PublishMessageAsync(object message, string queueName)
    {

        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSetting.Host,
            UserName = _rabbitMqSetting.UserName,
            Password = _rabbitMqSetting.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var messageJson = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        await Task.Run(() => channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body));
    }

    
    public async Task PublishAsync<T>(T message, string routeKey) where T : class
    {
        var messageJson = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(messageJson);
        
        await Task.Run(() =>
        {
            _channel.BasicPublish(
                exchange: "", 
                routingKey: routeKey, 
                basicProperties: null, 
                body: body);
        });
        Console.WriteLine("Message sent");
    }
}