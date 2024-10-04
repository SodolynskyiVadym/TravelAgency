﻿using AutoMapper;
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

    public PayController(TravelDbContext context, IMapper mapper, IOptions<AddressSetting> addressSetting, 
        IConnectionMultiplexer redis, IRabbitMqPublisher rabbitMqPublisher)
    {
        _userService = new UserService(context, mapper);
        _tourService = new TourService(context, mapper, redis);
        _paymentService = new PaymentService(context, mapper);
        _stripeHelper = new StripeHelper(addressSetting.Value);
        _mapper = mapper;
        _rabbitMqPublisher = rabbitMqPublisher;
        _addressSetting = addressSetting.Value;
    }
    
    [HttpGet("getTourFreeSeats/{id}")]
    public async Task<int> GetTourFreeSeats(int id)
    {
        Tour? tour = await _tourService.GetByIdAsync(id);
        if (tour == null) return 0;
        
        List<Payment> payments = await _paymentService.GetByTourId(id);
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
        return true;
    }
    

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
        
        TourEmailDto tourEmail = _mapper.Map<TourEmailDto>((tour, user.Email, paymentData.Quantity));
        await _rabbitMqPublisher.PublishAsync(tourEmail, "payment-queue");
        
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
        if(payment == null) return StatusCode(400, "Payment not found!");
        
        Tour? tour = await _tourService.GetByIdAsync(payment.TourId);
        if (tour == null) return StatusCode(400, "Tour not found!");
        User? user = await _userService.GetByIdAsync(payment.UserId);
        if (user == null) return StatusCode(400, "User not found!");

        payment.IsPaid = true;
        await _paymentService.UpdateAsync(_mapper.Map<PaymentDto>(payment));
        
        return Redirect($"{_addressSetting.Client}/tour/{payment.TourId}");
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
        return Redirect($"{_addressSetting.Client}/tour/{payment.TourId}");
    }
    
    public async Task DeleteUnpaidPaymentsAsync()
    {
        await _paymentService.DeleteUnpaid();
    }
}