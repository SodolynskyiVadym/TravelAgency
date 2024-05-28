using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI;

public class MyDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Transport> Transports { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tour>()
            .HasMany(t => t.Destinations)
            .WithOne(d => d.Tour)
            .HasForeignKey(d => d.TourId)
            .HasPrincipalKey(t => t.Id);
        
        
        modelBuilder.Entity<Destination>()
            .HasOne(d => d.Hotel)
            .WithMany()
            .HasForeignKey(d => d.HotelId)
            .HasPrincipalKey(h => h.Id);


        modelBuilder.Entity<Destination>()
            .HasOne(d => d.Transport)
            .WithMany()
            .HasForeignKey(d => d.TransportId)
            .HasPrincipalKey(t => t.Id);


        modelBuilder.Entity<Hotel>()
            .HasOne(h => h.Place)
            .WithMany()
            .HasForeignKey(h => h.PlaceId);
        
        
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .HasPrincipalKey(u => u.Id);

        
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Tour)
            .WithMany()
            .HasForeignKey(p => p.TourId)
            .HasPrincipalKey(t => t.Id);

        
        // modelBuilder.Entity<Destination>()
        //     .HasOne(d => d.Hotel)
        //     .WithMany()
        //     .HasForeignKey("HotelId");
        //
        // modelBuilder.Entity<Destination>()
        //     .HasOne(d => d.Transport)
        //     .WithMany()
        //     .HasForeignKey("TransportId");
        //
        // modelBuilder.Entity<Destination>()
        //     .HasOne(d => d.Tour)
        //     .WithMany(t => t.Destinations)
        //     .HasForeignKey("TourId");

    }
}