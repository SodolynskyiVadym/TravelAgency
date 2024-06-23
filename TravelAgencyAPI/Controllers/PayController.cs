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
    private readonly UserService _userService;
    private readonly TourService _tourService;
    private readonly PaymentService _paymentService;
    private readonly StripeHelper _stripeHelper;
    private readonly MailHelper _mailHelper;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public PayController(TravelDbContext context, IMapper mapper, IConfiguration config,
        IOptions<MailSetting> mailSetting, IConnectionMultiplexer redis)
    {
        _userService = new UserService(context, mapper);
        _tourService = new TourService(context, mapper, redis);
        _paymentService = new PaymentService(context, mapper);
        _mailHelper = new MailHelper(mailSetting);
        _stripeHelper = new StripeHelper(config);
        _mapper = mapper;
        _config = config;
    }
    
    [HttpGet("getTourFreeSeats/{id}")]
    public async Task<int> GetTourFreeSeats(int id)
    {
        Tour? tour = await _tourService.GetByIdAsync(id);
        if (tour == null) return 0;
        
        List<Payment> payments = await _paymentService.GetByTourId(id);
        Console.WriteLine(tour.QuantitySeats);
        Console.WriteLine(payments.Sum(p => p.Amount));
        return tour.QuantitySeats - payments.Sum(p => p.Amount);
    }
    
    
    [Authorize]
    [HttpGet("getUserPayments")]
    public async Task<List<Payment>> GetUserPayments()
    {
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        return await _paymentService.GetByUserId(userId);
    }
    
    
    [Authorize]
    [HttpGet("haveUserPayment/{tourId}")]
    public async Task<bool> GetUserPayment(int tourId)
    {
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        Payment? payment = await _paymentService.GetByUserIdTourId(userId, tourId);
        if (payment == null) return false;
        else return true;
    }
    

    [Authorize]
    [HttpPost("reserveTour")]
    public async Task<IActionResult> ReserveTour(PaymentDataDto paymentData)
    {
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        User? user = await _userService.GetByIdAsync(userId);
        Tour? tour = await _tourService.GetByIdAsync(paymentData.TourId);
        if (user == null || tour == null) return BadRequest("User or tour not found!");
        
        Payment? paymentFromDb = await _paymentService.GetByUserIdTourId(userId, tour.Id);
        if (paymentFromDb != null)
        {
            Console.WriteLine("Payment already exists!");
            return BadRequest("Payment already exists!");
        }

        PaymentDto payment = new PaymentDto
        {
            UserId = userId,
            TourId = paymentData.TourId,
            Amount = paymentData.Quantity,
            Date = DateTime.Now,
            IsPaid = false
        };
        int paymentId = await _paymentService.AddAsync(payment);
        string sessionId = await _stripeHelper.CreateStripeSession(paymentData, payment, tour, paymentId);
        
        payment.Id = paymentId;
        payment.StripeSession = sessionId;
        await _paymentService.UpdateAsync(payment);
        return Ok(sessionId);
    }


    [HttpGet("success/{sessionId}")]
    public async Task<IActionResult> CheckoutSuccess(string sessionId)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(sessionId);

        string? paymentId = session.Metadata["PaymentId"];
        if (paymentId == null) return StatusCode(400, "Payment id not found!");

        Payment? payment = await _paymentService.GetByIdAsync(int.Parse(paymentId));
        if (payment == null) return StatusCode(400, "Payment not found!");

        payment.IsPaid = true;
        await _paymentService.UpdateAsync(_mapper.Map<PaymentDto>(payment));

        return Redirect(_config.GetSection("Urls:Client").Value);
    }


    [HttpGet("failure/{sessionId}")]
    public async Task<IActionResult> CheckoutFailure(string sessionId)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(sessionId);

        string? paymentId = session.Metadata["PaymentId"];
        if (paymentId == null) return StatusCode(400, "Payment id not found!");

        Payment? payment = await _paymentService.GetByIdAsync(int.Parse(paymentId));
        if (payment == null) return StatusCode(400, "Payment not found!");

        await _paymentService.DeleteAsync(payment.Id);
        return Redirect(_config.GetSection("Urls:Client").Value + "/error");
    }
    
    public async Task DeleteUnpaidPaymentsAsync()
    {
        await _paymentService.DeleteUnpaid();
    }
}