using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;

namespace AuctionAPI_20_BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public List<IdentityUser> SearchByEmail(string email)
    {
        return _userRepository.SearchByEmail(email);
    }

    public List<Auction> GetWonAuctions(string userId)
    {
        return _userRepository.GetWonAuctions(userId);
    }
}