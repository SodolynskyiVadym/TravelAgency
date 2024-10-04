using TravelAgencyAPIServer.Helpers;

namespace TravelAgencyAPIServer.Settings;

public class RabbitMqSetting
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public IEnumerable<RabbitMqQueueSetting> Queues { get; set; }
    
}