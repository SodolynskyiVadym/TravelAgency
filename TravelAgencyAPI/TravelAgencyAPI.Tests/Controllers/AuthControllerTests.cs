using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TravelAgencyAPIServer.Controllers;
using TravelAgencyAPIServer.DTO;
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
    public async Task AuthController_GetUserById_ReturnsUser()
    {
        // Arrange
        var id = 1;

        // Act
        var result = await _authController.GetUserById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("user1@example.com", result.Email);
    }

    
    [Theory]
    [InlineData("admin@example.com", "12345678", true)]
    [InlineData("user1@example.com", "wrongpassword", false)]
    [InlineData("user@example.com", "wrongpassword", false)]
    public async Task AuthController_Login_ReturnsExpectedResult(string email, string password, bool isSuccess)
    {
        // Arrange
        UserEmailPasswordDto user = new UserEmailPasswordDto
        {
            Email = email,
            Password = password
        };

        // Act
        var result = await _authController.Login(user);

        // Assert
        if (isSuccess)
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
        else
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult?.StatusCode.Should().Be(400);
        }
    }


    [Theory]
    [InlineData("admin@example.com", "12345678", true)]
    [InlineData("user@example.com", "12345678", false)]
    [InlineData("user@example.com", "wrongpassword", false)]
    public async Task AuthController_LoginViaReservePassword_ReturnsOk(string email, string password, bool isSuccess)
    {
        // Arrange
        var user = new UserEmailPasswordDto()
        {
            Email = email,
            Password = password
        };
        
        // Act
        var result = await _authController.LoginViaReservePassword(user);
        
        //Assert
        if (isSuccess)
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
        else
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult?.StatusCode.Should().Be(400);
        }
    }
}