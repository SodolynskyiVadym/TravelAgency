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
    private readonly List<RabbitMqQueueSetting> _rabbitMqQueues;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqPublisher(IOptions<List<RabbitMqQueueSetting>> rabbitMqQueues, string connectionString)
    {
        _rabbitMqQueues = rabbitMqQueues.Value;
        
        var factory = new ConnectionFactory()
        {
            Uri = new Uri(connectionString)
        };
        Console.WriteLine(connectionString);
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        foreach (var queue in _rabbitMqQueues)
        {
            _channel.QueueDeclare(queue: queue.Name, durable: queue.Durable, exclusive: queue.Exclusive,
                autoDelete: queue.AutoDelete, arguments: null);
        }
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