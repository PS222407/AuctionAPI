namespace AuctionAPI_10_Api.ViewModels;

public class OrderViewModel
{
    public string Id { get; set; }

    public int PriceInCents { get; set; }

    public string PaymentStatus { get; set; }
}