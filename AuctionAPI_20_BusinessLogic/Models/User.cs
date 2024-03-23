using System.ComponentModel.DataAnnotations;

namespace AuctionAPI_20_BusinessLogic.Models;

public class User
{
    [StringLength(255)] public string Id { get; init; } = null!;

    [StringLength(255)] public string Email { get; set; } = null!;

    [StringLength(255)] public string Password { get; set; } = null!;

    public List<Role> Roles { get; set; } = [];

    public List<RefreshToken> RefreshTokens { get; set; } = [];
}