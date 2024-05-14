
using Microsoft.EntityFrameworkCore;
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
    
    // public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    // {
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=travel_agency;User Id=sa;Password=Test123456;TrustServerCertificate=true");
    }

    protected MyDbContext()
    {
    }
}