using Microsoft.EntityFrameworkCore;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Models;

namespace TravelAgencyAPI.Tests;

public class DbContextInit
{
    private static readonly IEnumerable<Place> Places = new List<Place>
    {
        new()
        {
            Id = 1,
            Name = "Paris",
            Country = "France",
            Description = "The capital city of France, known for the Eiffel Tower.",
            ImagesUrls = new List<PlaceImageUrl>
            {
                new() { Url = "https://example.com/paris1.jpg" },
                new() { Url = "https://example.com/paris2.jpg" }
            }
        },
        new()
        {
            Id = 2,
            Name = "New York",
            Country = "USA",
            Description = "A major city in the USA, known for the Statue of Liberty.",
            ImagesUrls = new List<PlaceImageUrl>
            {
                new() { Url = "https://example.com/ny1.jpg" },
                new() { Url = "https://example.com/ny2.jpg" }
            }
        },
        new()
        {
            Id = 3,
            Name = "Tokyo",
            Country = "Japan",
            Description = "The capital city of Japan, known for its modern architecture and culture.",
            ImagesUrls = new List<PlaceImageUrl>
            {
                new() { Url = "https://example.com/tokyo1.jpg" },
                new() { Url = "https://example.com/tokyo2.jpg" }
            }
        },
        new()
        {
            Id = 4,
            Name = "Sydney",
            Country = "Australia",
            Description = "A major city in Australia, known for the Sydney Opera House.",
            ImagesUrls = new List<PlaceImageUrl>
            {
                new() { Url = "https://example.com/sydney1.jpg" },
                new() { Url = "https://example.com/sydney2.jpg" }
            }
        }
    };

    private static readonly IEnumerable<Hotel> Hotels = new List<Hotel>
    {
        new()
        {
            Id = 1,
            Name = "Hotel Paris",
            Address = "123 Paris St, Paris, France",
            Description = "A luxurious hotel in Paris.",
            PricePerNight = 200,
            ImageUrl = "https://example.com/hotelparis.jpg",
            PlaceId = 1
        },
        new()
        {
            Id = 2,
            Name = "Hotel New York",
            Address = "456 New York Ave, New York, USA",
            Description = "A modern hotel in New York.",
            PricePerNight = 250,
            ImageUrl = "https://example.com/hotelnewyork.jpg",
            PlaceId = 2
        },
        new()
        {
            Id = 3,
            Name = "Hotel Tokyo",
            Address = "789 Tokyo Rd, Tokyo, Japan",
            Description = "A traditional hotel in Tokyo.",
            PricePerNight = 300,
            ImageUrl = "https://example.com/hoteltokyo.jpg",
            PlaceId = 3
        },
        new()
        {
            Id = 4,
            Name = "Hotel Sydney",
            Address = "101 Sydney Blvd, Sydney, Australia",
            Description = "A beachfront hotel in Sydney.",
            PricePerNight = 220,
            ImageUrl = "https://example.com/hotelsydney.jpg",
            PlaceId = 4
        }
    };

    private static readonly IEnumerable<Transport> Transports = new List<Transport>
    {
        new()
        {
            Id = 1,
            Name = "Air France Flight",
            Description = "Flight from Paris to New York.",
            Type = "Flight",
            QuantitySeats = 300,
            ImageUrl = "https://example.com/airfrance.jpg"
        },
        new()
        {
            Id = 2,
            Name = "Japan Railways Train",
            Description = "Train from Tokyo to Kyoto.",
            Type = "Train",
            QuantitySeats = 500,
            ImageUrl = "https://example.com/japanrailways.jpg"
        }
    };

    private static readonly IEnumerable<Tour> Tours = new List<Tour>
    {
        new()
        {
            Id = 1,
            Name = "Reef Explorer",
            Description = "A 5-day tour exploring the Great Barrier Reef.",
            StartDate = new DateTime(2025, 6, 1),
            EndDate = new DateTime(2025, 6, 5),
            Price = 1500,
            QuantitySeats = 20,
            ImageUrl = "https://example.com/reefexplorer.jpg",
            IsAvailable = true,
            PlaceStartId = 1,
            PlaceEndId = 2,
            TransportToEndId = 1
        }
    };

    private static readonly IEnumerable<Destination> Destinations = new List<Destination>
    {
        new()
        {
            Id = 1,
            StartDate = new DateTime(2025, 6, 1),
            EndDate = new DateTime(2025, 6, 5),
            TourId = 1,
            HotelId = 1,
            TransportId = 1
        }
    };


    public static async Task<TravelDbContext> GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<TravelDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new TravelDbContext(options);
        await databaseContext.Database.EnsureCreatedAsync();

        foreach (var place in Places) databaseContext.Places.Add(place);
        await databaseContext.SaveChangesAsync();

        foreach (var transport in Transports) databaseContext.Transports.Add(transport);
        await databaseContext.SaveChangesAsync();

        foreach (var hotel in Hotels) databaseContext.Hotels.Add(hotel);
        await databaseContext.SaveChangesAsync();

        foreach (var tour in Tours) databaseContext.Tours.Add(tour);
        await databaseContext.SaveChangesAsync();

        foreach (var destination in Destinations) databaseContext.Destinations.Add(destination);
        await databaseContext.SaveChangesAsync();

        return databaseContext;
    }
}