using AuctionAPI_20_BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuctionAPI_30_DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public List<IdentityUser> SearchByEmail(string email)
    {
        return _userManager.Users.Where(u => u.Email != null && u.Email.Contains(email)).ToList();
    }
}