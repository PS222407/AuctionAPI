using AuctionAPI_20_BusinessLogic.Enums;

namespace AuctionAPI_20_BusinessLogic.Models;

public class Order
{
    public string Id { get; set; }

    public string ExternalPaymentId { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }

    public long ProductId { get; set; }

    public Product? Product { get; set; }

    public int PriceInCents { get; set; }

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
}