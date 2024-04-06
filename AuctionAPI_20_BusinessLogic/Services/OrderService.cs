using AuctionAPI_20_BusinessLogic.Exceptions;
using AuctionAPI_20_BusinessLogic.Helpers;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using Mollie.Api.Models.Payment.Response;

namespace AuctionAPI_20_BusinessLogic.Services;

public class OrderService(IOrderRepository orderRepository, IMollieHelper mollieHelper) : IOrderService
{
    public Order? GetBy(string id)
    {
        return orderRepository.GetBy(id);
    }

    public Order Create(Product product, string userId, string paymentId, string orderId)
    {
        Order order = new()
        {
            Id = orderId,
            ExternalPaymentId = paymentId,
            UserId = userId,
            ProductId = (int)product.Id,
            PriceInCents = product.PriceInCents,
        };

        Order? createdOrder = orderRepository.Create(order);
        if (createdOrder == null)
        {
            throw new DatabaseCreationException("Order could not be created");
        }

        return createdOrder;
    }

    public async Task<PaymentResponse> CreateMolliePayment(int totalAmount, string orderId)
    {
        return await mollieHelper.CreatePayment(totalAmount, orderId);
    }

    public async Task<PaymentResponse> GetPaymentFromMollie(string externalPaymentId)
    {
        return await mollieHelper.GetPayment(externalPaymentId);
    }

    public bool Update(Order order)
    {
        return orderRepository.Update(order);
    }

    public Order? GetByExternalPaymentId(string id)
    {
        return orderRepository.GetByExternalPaymentId(id);
    }

    public List<Order> GetByUserId(string userId)
    {
        return orderRepository.GetByUserId(userId);
    }
}