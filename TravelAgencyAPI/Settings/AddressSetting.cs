namespace TravelAgencyAPI.Settings;

public class AddressSetting
{
    public string Server { get; set; }
    public string Client { get; set; }

    public override string ToString()
    {
        return $"Server address - {Server} Client address - {Client}";
    }
}