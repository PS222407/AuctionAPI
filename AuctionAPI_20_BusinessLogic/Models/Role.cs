namespace AuctionAPI_20_BusinessLogic.Models;

public class Role
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public List<User> Users { get; set; } = [];
}