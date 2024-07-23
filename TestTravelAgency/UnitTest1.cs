using AutoMapper;
using Moq;
using StackExchange.Redis;
using TravelAgencyAPI.Controllers;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;

namespace TestTravelAgency;

public class Tests
{
    private Mock<TravelDbContext> _mockContext;
    private Mock<IMapper> _mockMapper;
    private Mock<IConnectionMultiplexer> _mockRedis;
    private PlaceController _controller;

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<TravelDbContext>();
        _mockMapper = new Mock<IMapper>();
        _mockRedis = new Mock<IConnectionMultiplexer>();

        _controller = new PlaceController(_mockContext.Object, _mockMapper.Object, _mockRedis.Object);
    }

    [Test]
    public async Task GetPlace_ReturnsCorrectPlace()
    {
        // Arrange
        var expectedPlace = new Place { Id = 1, Name = "Test Place" };
        // _mockContext.Setup(service => service.GetByIdAsync(1))
        //     .ReturnsAsync(expectedPlace);
        _mockContext.Setup(service => service.Places.FindAsync(1))
            .ReturnsAsync(expectedPlace);

        // Act
        var result = await _controller.GetPlace(1);

        // Assert
        Assert.That(result, Is.EqualTo(expectedPlace));
    }
}