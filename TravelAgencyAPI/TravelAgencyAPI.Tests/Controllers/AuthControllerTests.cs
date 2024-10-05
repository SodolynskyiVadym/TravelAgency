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
        _context = DbContextInit.GetDatabaseContext().Result;
        _mapper = (new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()))).CreateMapper();
        _rabbitMqPublisher = A.Fake<IRabbitMqPublisher>();
        _authController = new AuthController(_context, _mapper, _authSettingOptions, _rabbitMqPublisher);
    }
}