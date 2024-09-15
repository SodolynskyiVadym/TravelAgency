using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using TravelAgencyAPI.Settings;

namespace TravelAgencyAPI.Helpers;

public class RabbitMqPublisher
{
    private readonly RabbitMqSetting _rabbitMqSetting;

    public RabbitMqPublisher(IOptions<RabbitMqSetting> rabbitMqSetting)
    {
        _rabbitMqSetting = rabbitMqSetting.Value;
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
}