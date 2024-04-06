using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IOrderRepository
{
    public Order? Create(Order order);

    public Order? GetBy(string id);

    public bool Update(Order order);

    public Order? GetByExternalPaymentId(string id);

    public List<Order> GetByUserId(string userId);
}