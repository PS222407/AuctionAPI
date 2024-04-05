using AuctionAPI_20_BusinessLogic.Models;
using Mollie.Api.Models.Payment.Response;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IOrderService
{
    public Order? GetBy(string id);

    public Order Create(Product product, string userId, string paymentId, string orderId);

    public Task<PaymentResponse> CreateMolliePayment(int totalAmount, string orderId);

    public Task<PaymentResponse> GetPaymentFromMollie(string externalPaymentId);

    public bool Update(Order order);

    public Order? GetByExternalPaymentId(string externalPaymentId);
}