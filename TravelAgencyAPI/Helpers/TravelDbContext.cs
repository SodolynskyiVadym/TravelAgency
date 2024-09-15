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
        modelBuilder.Entity<Hotel>()
            .HasIndex(h => new { h.Name, h.PlaceId, h.Address })
            .IsUnique();

        modelBuilder.Entity<Payment>()
            .HasIndex(p => new { p.UserId, p.TourId })
            .IsUnique();

        modelBuilder.Entity<Review>()
            .HasIndex(r => new { r.UserId, r.TourId })
            .IsUnique();
            
        
        modelBuilder.Entity<Tour>()
            .HasMany(t => t.Destinations)
            .WithOne()
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
        
        modelBuilder.Entity<Place>()
            .HasMany(p => p.ImagesUrls)
            .WithOne()
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
            .WithMany()
            .HasForeignKey(t => t.PlaceStartId)
            .HasPrincipalKey(p => p.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tour>()
            .HasOne(t => t.PlaceEnd)
            .WithMany()
            .HasForeignKey(t => t.PlaceEndId)
            .HasPrincipalKey(p => p.Id)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Tour>()
            .HasOne(t => t.TransportToEnd)
            .WithMany()
            .HasForeignKey(t => t.TransportToEndId)
            .HasPrincipalKey(t => t.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}