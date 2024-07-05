using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Settings;

namespace TravelAgencyAPI.Helpers;
using Stripe.Checkout;

public class StripeHelper
{
    private readonly AddressSetting _addressSetting;
    public StripeHelper(AddressSetting addressSetting)
    {
        _addressSetting = addressSetting;
    }
    
    public async Task<string> CreateStripeSession(PaymentDataDto paymentData, Tour tour, int paymentId)
    {
        var options = new SessionCreateOptions
        {
            SuccessUrl = $"{_addressSetting.Server}/pay/success/{{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{_addressSetting.Server}/pay/failure/{{CHECKOUT_SESSION_ID}}",
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