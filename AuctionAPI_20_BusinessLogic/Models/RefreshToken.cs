namespace AuctionAPI_20_BusinessLogic.Models;

public class RefreshToken
{
    public string Id { get; set; }
    
    public string Token { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(30);
    
    public List<User> Users { get; set; } = [];
}