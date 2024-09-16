namespace TravelAgencyAPI.Services.Interfaces;

public interface IRabbitMqPublisher
{
    public void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey)
        where T : class;
    public Task PublishMessageAsync(object message, string queueName);
}