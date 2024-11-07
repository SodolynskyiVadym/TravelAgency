using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Stripe.Checkout;
using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Models;
using TravelAgencyAPIServer.Services;
using TravelAgencyAPIServer.Services.Interfaces;
using TravelAgencyAPIServer.Settings;

namespace TravelAgencyAPIServer.Controllers;

/// <summary>
/// The PayController class handles payment-related operations for the Travel Agency API.
/// </summary>
[ApiController]
[Route("[controller]")]
public class PayController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TourService _tourService;
    private readonly PaymentService _paymentService;
    private readonly StripeHelper _stripeHelper;
    private readonly AddressSetting _addressSetting;
    private readonly IMapper _mapper;
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    /// <summary>
    /// Initializes a new instance of the PayController class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="mapper">The object mapper.</param>
    /// <param name="addressSetting">The address settings.</param>
    /// <param name="redis">The Redis connection multiplexer.</param>
    /// <param name="rabbitMqPublisher">The RabbitMQ publisher.</param>
    /// <param name="dapperDbContext">The dapper context</param>
    public PayController(TravelDbContext context, IMapper mapper, IOptions<AddressSetting> addressSetting,
        IConnectionMultiplexer redis, IRabbitMqPublisher rabbitMqPublisher, DapperDbContext dapperDbContext)
    {
        _userService = new UserService(context, mapper);
        _tourService = new TourService(context, mapper, redis, dapperDbContext);
        _paymentService = new PaymentService(context, mapper);
        _stripeHelper = new StripeHelper(addressSetting.Value);
        _mapper = mapper;
        _rabbitMqPublisher = rabbitMqPublisher;
        _addressSetting = addressSetting.Value;
    }

    /// <summary>
    /// Retrieves the number of free seats available for a specific tour.
    /// </summary>
    /// <param name="id">The ID of the tour.</param>
    /// <returns>The number of free seats available.</returns>
    [HttpGet("getTourFreeSeats/{id}")]
    public async Task<int> GetTourFreeSeats(int id)
    {
        Tour? tour = await _tourService.GetByIdAsync(id);
        if (tour == null) return 0;

        List<Payment> payments = await _paymentService.GetByTourId(id);
        return tour.QuantitySeats - payments.Sum(p => p.Amount);
    }

    /// <summary>
    /// Retrieves the list of payments made by the authenticated user.
    /// </summary>
    /// <returns>A list of payments made by the user.</returns>
    [Authorize]
    [HttpGet("getUserPayments")]
    public async Task<List<Payment>> GetUserPayments()
    {
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        return await _paymentService.GetByUserId(userId);
    }

    /// <summary>
    /// Checks if the authenticated user has made a payment for a specific tour.
    /// </summary>
    /// <param name="tourId">The ID of the tour.</param>
    /// <returns>True if the user has made a payment for the tour, otherwise false.</returns>
    [Authorize]
    [HttpGet("haveUserPayment/{tourId}")]
    public async Task<bool> GetUserPayment(int tourId)
    {
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        Payment? payment = await _paymentService.GetByUserIdTourId(userId, tourId);
        if (payment == null) return false;
        return true;
    }

    /// <summary>
    /// Reserves a tour for the authenticated user and creates a Stripe payment session.
    /// </summary>
    /// <param name="paymentData">The payment data including tour ID and quantity.</param>
    /// <returns>The Stripe session ID if the reservation is successful.</returns>
    [Authorize]
    [HttpPost("reserveTour")]
    public async Task<IActionResult> ReserveTour(PaymentDataDto paymentData)
    {
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        User? user = await _userService.GetByIdAsync(userId);
        Tour? tour = await _tourService.GetByIdAsync(paymentData.TourId);
        if (user == null || tour == null) return BadRequest("User or tour not found!");

        PaymentDto payment = new PaymentDto
        {
            UserId = userId,
            TourId = paymentData.TourId,
            Amount = paymentData.Quantity,
            Date = DateTime.Now,
            IsPaid = false
        };
        int paymentId = await _paymentService.AddAsync(payment);
        if(paymentId == 0) return StatusCode(400, "User already have this tour!");
        string sessionId = await _stripeHelper.CreateStripeSession(paymentData, tour, paymentId);

        payment.Id = paymentId;
        payment.StripeSession = sessionId;
        await _paymentService.UpdateAsync(payment);

        return Ok(new { sessionId });;
    }

    /// <summary>
    /// Handles the successful completion of a payment.
    /// </summary>
    /// <param name="sessionId">The Stripe session ID.</param>
    /// <returns>Redirects to the tour page on the client application.</returns>
    [HttpGet("success/{sessionId}")]
    public async Task<IActionResult> CheckoutSuccess(string sessionId)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(sessionId);

        string? paymentId = session.Metadata["PaymentId"];
        if (paymentId == null) return StatusCode(400, "Payment id not found!");

        Payment? payment = await _paymentService.GetByIdAsync(int.Parse(paymentId));
        if(payment == null) return StatusCode(400, "Payment not found!");

        Tour? tour = await _tourService.GetByIdAsync(payment.TourId);
        if (tour == null) return StatusCode(400, "Tour not found!");
        User? user = await _userService.GetByIdAsync(payment.UserId);
        if (user == null) return StatusCode(400, "User not found!");

        payment.IsPaid = true;
        await _paymentService.UpdateAsync(_mapper.Map<PaymentDto>(payment));
        
        TourEmailDto tourEmail = _mapper.Map<TourEmailDto>((tour, user.Email, payment.Amount));
        await _rabbitMqPublisher.PublishAsync(tourEmail, "payment-queue");

        return Redirect($"{_addressSetting.Client}/tour/{payment.TourId}");
    }

    /// <summary>
    /// Handles the failure of a payment.
    /// </summary>
    /// <param name="sessionId">The Stripe session ID.</param>
    /// <returns>Redirects to the tour page on the client application.</returns>
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
        return Redirect($"{_addressSetting.Client}/tour/{payment.TourId}");
    }

    /// <summary>
    /// Deletes unpaid payments from the database.
    /// </summary>
    /// <returns>None (asynchronous operation).</returns>
    public async Task DeleteUnpaidPaymentsAsync()
    {
        await _paymentService.DeleteUnpaid();
    }
}