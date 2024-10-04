namespace TravelAgencyAPIServer.DTO;

public class TourForeignKeyDto
{
    public int PlaceStartId { get; set; }
    public int PlaceEndId { get; set; }
    
    public int TransportToEndId { get; set; }
}