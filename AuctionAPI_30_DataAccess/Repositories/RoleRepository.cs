using AuctionAPI_20_BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuctionAPI_30_DataAccess.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IEnumerable<IdentityRole> GetAll()
    {
        return _roleManager.Roles;
    }

    public async Task<IdentityUser?> AttachRoleToUser(string roleName, string userId)
    {
        IdentityUser? user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
        
        return user;
    }

    public async Task<IdentityUser?> RevokeRoleFromUser(string roleName, string userId)
    {
        IdentityUser? user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }
        
        return user;
    }
}