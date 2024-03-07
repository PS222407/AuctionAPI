using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;

namespace AuctionAPI_30_DataAccess.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public RoleRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public IEnumerable<IdentityRole> GetAll()
    {
        return _roleManager.Roles;
    }

    public async Task AttachRoleToUser(string roleName, string userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, roleName);
            await _signInManager.RefreshSignInAsync(user);
        }
    }

    public async Task RevokeRoleFromUser(string roleName, string userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
            await _signInManager.RefreshSignInAsync(user);
        }
    }
}