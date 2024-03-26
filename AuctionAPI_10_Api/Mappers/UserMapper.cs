using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_10_Api.Mappers;

public static class UserMapper
{
    public static UserViewModel MapToViewModel(User user)
    {
        return new UserViewModel
        {
            Id = user.Id,
            Email = user.Email,
        };
    }
}