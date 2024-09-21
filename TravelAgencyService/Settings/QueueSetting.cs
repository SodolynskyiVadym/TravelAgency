namespace TravelAgencyService.Settings;

public class QueueSetting
{
    public bool Durable { get; set; }
    public bool Exclusive { get; set; }
    public bool AutoDelete { get; set; }
    public bool AutoAck { get; set; }
}