using Mollie.Api.Models.Payment.Response;

namespace AuctionAPI_20_BusinessLogic.Helpers;

public interface IMollieHelper
{
    public Task<PaymentResponse> CreatePayment(int totalAmount, string orderId);

    public Task<PaymentResponse> GetPayment(string externalPaymentId);
}