using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories;

public class PaymentRepository : IRepository<Payment, PaymentDto>, IPaymentRepository
{
    private readonly TravelDbContext _context;
    private readonly IMapper _mapper;

    public PaymentRepository(TravelDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<List<Payment>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task<int> AddAsync(PaymentDto paymentDto)
    {
        Payment payment = _mapper.Map<Payment>(paymentDto);
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
        return payment.Id;
    }

    public async Task<bool> UpdateAsync(PaymentDto paymentDto)
    {
        Payment? payment = await _context.Payments.FindAsync(paymentDto.Id);
        if (payment == null) return false;
        
        payment.UserId = paymentDto.UserId;
        payment.TourId = paymentDto.TourId;
        payment.Amount = paymentDto.Amount;
        payment.Date = DateTime.Now;
        payment.IsPaid = paymentDto.IsPaid;
        payment.StripeSession ??= paymentDto.StripeSession;
        
        await _context.SaveChangesAsync();
        return true;
    }
    

    public async Task<bool> DeleteAsync(int id)
    {
        Payment? payment = await _context.Payments.FindAsync(id);
        if(payment == null) return false;
        
        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Payment?> GetByUserIdTourId(int userId, int tourId)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.UserId == userId && p.TourId == tourId);
    }

    public async Task<List<Payment>> GetByUserId(int userId)
    {
        return await _context.Payments
            .Include(p => p.Tour)
            .Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task DeleteUnpaid()
    {
        List<Payment> payments = await _context.Payments.Where(p => !p.IsPaid).ToListAsync();
        _context.Payments.RemoveRange(payments);
        await _context.SaveChangesAsync();
    }
}