using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_10_Api.Mappers;

public static class OrderMapper
{
    public static OrderViewModel MapToViewModel(Order order, IConfiguration configuration)
    {
        if (order.Product != null)
        {
            order.Product.Orders = null;
        }

        return new OrderViewModel
        {
            Id = order.Id,
            PriceInCents = order.PriceInCents,
            PaymentStatus = order.PaymentStatus.ToString(),
            Product = order.Product == null ? null : ProductMapper.MapToViewModel(order.Product, configuration),
        };
    }
}