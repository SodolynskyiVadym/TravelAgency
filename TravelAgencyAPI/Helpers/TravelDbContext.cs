using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Helpers;

public class TravelDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Transport> Transports { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PlaceImageUrl> PlaceImageUrls { get; set; }
    public DbSet<Review> Reviews { get; set; }

    public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Destination>()
            .HasOne(d => d.Tour)
            .WithMany()
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
            .HasForeignKey(h => h.PlaceId)
            .HasPrincipalKey(p => p.Id);
        
        
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .HasPrincipalKey(u => u.Id);

        
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Tour)
            .WithMany()
            .HasForeignKey(p => p.TourId)
            .HasPrincipalKey(t => t.Id);

        modelBuilder.Entity<PlaceImageUrl>()
            .HasOne(i => i.Place)
            .WithMany(p => p.ImagesUrls)
            .HasForeignKey(i => i.PlaceId)
            .HasPrincipalKey(p => p.Id);
        
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .HasPrincipalKey(u => u.Id);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Tour)
            .WithMany()
            .HasForeignKey(r => r.TourId)
            .HasPrincipalKey(t => t.Id);

        modelBuilder.Entity<Tour>()
            .HasOne(t => t.PlaceStart)
            .WithOne()
            .HasForeignKey<Tour>(t => t.PlaceStartId)
            .HasPrincipalKey<Place>(p => p.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tour>()
            .HasOne(t => t.PlaceEnd)
            .WithOne()
            .HasForeignKey<Tour>(t => t.PlaceEndId)
            .HasPrincipalKey<Place>(p => p.Id)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Tour>()
            .HasOne(t => t.TransportToEnd)
            .WithOne()
            .HasForeignKey<Tour>(t => t.TransportToEndId)
            .HasPrincipalKey<Transport>(t => t.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}