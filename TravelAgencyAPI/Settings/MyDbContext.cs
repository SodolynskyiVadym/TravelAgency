using System.Data.Entity;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Settings;

public class MyDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Transport> Transports { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<Payment> Payments { get; set; }
}