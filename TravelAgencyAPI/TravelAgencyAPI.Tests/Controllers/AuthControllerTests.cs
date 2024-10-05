using AutoMapper;
using FakeItEasy;
using Microsoft.Extensions.Options;
using TravelAgencyAPIServer.Controllers;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Services.Interfaces;
using TravelAgencyAPIServer.Settings;

namespace TravelAgencyAPI.Tests.Controllers;

public class AuthControllerTests
{
    private readonly IMapper _mapper;
    private readonly IRabbitMqPublisher _rabbitMqPublisher;
    private readonly TravelDbContext _context;
    private readonly IOptions<AuthSetting> _authSettingOptions = Options.Create(new AuthSetting
    {
        PasswordKey = "passwordpasswordpasswordpasswordpasswordpasswordpasswordpasswordpasswordpasswordpassword",
        TokenKey = "tokentokentokentokentokentokentokentokentokentokentokentokentokentokentokentokentokentoken"
    });
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        _mapper = (new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()))).CreateMapper();
        var dbContextInit = new DbContextInit(new AuthHelper(_authSettingOptions.Value), _mapper);
        _context = dbContextInit.GetDatabaseContext().Result;
        _rabbitMqPublisher = A.Fake<IRabbitMqPublisher>();
        _authController = new AuthController(_context, _mapper, _authSettingOptions, _rabbitMqPublisher);
    }
    
    
    [Fact]
    public async Task GetUserById_WithValidId_ReturnsUser()
    {
        // Arrange
        var id = 1;

        // Act
        var result = await _authController.GetUserById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("user1@example.com", result.Email);
    }
}