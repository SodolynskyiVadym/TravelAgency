using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;
using TravelAgencyAPI.Settings;
using Stripe.Checkout;

namespace TravelAgencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PayController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly TourRepository _tourRepository;
    private readonly PaymentRepository _paymentRepository;
    private readonly StripeHelper _stripeHelper;
    private readonly MailHelper _mailHelper;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public PayController(TravelDbContext context, IMapper mapper, IConfiguration config,
        IOptions<MailSetting> mailSetting, IConnectionMultiplexer redis)
    {
        _userRepository = new UserRepository(context, mapper);
        _tourRepository = new TourRepository(context, mapper, redis);
        _paymentRepository = new PaymentRepository(context, mapper);
        _mailHelper = new MailHelper(mailSetting);
        _stripeHelper = new StripeHelper(config);
        _mapper = mapper;
        _config = config;
    }

    [Authorize]
    [HttpPost("reserveTour")]
    public async Task<string> ReserveTour(PaymentDataDto paymentData)
    {
        Console.WriteLine("Code work");
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return "0";

        Tour? tour = await _tourRepository.GetByIdAsync(paymentData.TourId);
        if (tour == null) return "0";

        PaymentDto payment = new PaymentDto
        {
            UserId = userId,
            TourId = paymentData.TourId,
            Amount = paymentData.Quantity,
            Date = DateTime.Now,
            IsPaid = false
        };
        int paymentId = await _paymentRepository.AddAsync(payment);
        
        string sessionId = await _stripeHelper.CreateStripeSession(paymentData, payment, tour, paymentId);

        return sessionId;
    }


    [HttpGet("success/{sessionId}")]
    public async Task<IActionResult> CheckoutSuccess(string sessionId)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(sessionId);

        string? paymentId = session.Metadata["PaymentId"];
        if (paymentId == null) return StatusCode(400, "Payment id not found!");

        Payment? payment = await _paymentRepository.GetByIdAsync(int.Parse(paymentId));
        if (payment == null) return StatusCode(400, "Payment not found!");

        payment.IsPaid = true;
        await _paymentRepository.UpdateAsync(payment.Id, _mapper.Map<PaymentDto>(payment));

        return Redirect(_config.GetSection("Urls:Client").Value);
    }


    [HttpGet("failure/{sessionId}")]
    public async Task<IActionResult> CheckoutFailure(string sessionId)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(sessionId);

        string? paymentId = session.Metadata["PaymentId"];
        if (paymentId == null) return StatusCode(400, "Payment id not found!");

        Payment? payment = await _paymentRepository.GetByIdAsync(int.Parse(paymentId));
        if (payment == null) return StatusCode(400, "Payment not found!");

        await _paymentRepository.DeleteAsync(payment.Id);
        return Redirect(_config.GetSection("Urls:Client").Value + "/error");
    }
    
    public async Task DeleteUnpaidPaymentsAsync()
    {
        await _paymentRepository.DeleteUnpaid();
    }
}