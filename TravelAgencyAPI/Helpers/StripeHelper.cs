using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Helpers;
using Stripe.Checkout;

public class StripeHelper
{
    private readonly IConfiguration _config;
    public StripeHelper(IConfiguration config)
    {
        _config = config;
    }
    
    public async Task<string> CreateStripeSession(PaymentDataDto paymentData, PaymentDto payment, Tour tour, int paymentId)
    {
        string? serverUrl = _config.GetSection("Urls:Server").Value;
        
        var options = new SessionCreateOptions
        {
            SuccessUrl = $"{serverUrl}/pay/success/{{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{serverUrl}/pay/failure/{{CHECKOUT_SESSION_ID}}",
            PaymentMethodTypes = ["card"],
            Metadata = new Dictionary<string, string>
                {
                    { "PaymentId", paymentId.ToString()}
                },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = tour.Price * 100,
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = tour.Name,
                            Images = new List<string> { tour.ImageUrl }
                        },
                    },
                    Quantity = paymentData.Quantity
                }
            },
            Mode = "payment"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return session.Id;
    }
}