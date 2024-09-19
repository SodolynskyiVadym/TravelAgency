namespace TravelAgencyAPI.Services.Interfaces;

public interface IRabbitMqPublisher
{
    public Task PublishAsync<T>(T message, string routeKey)
        where T : class;
}