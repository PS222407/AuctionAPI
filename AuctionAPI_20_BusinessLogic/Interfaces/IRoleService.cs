using Microsoft.AspNetCore.Identity;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IRoleService
{
    public IEnumerable<IdentityRole> GetAll();

    public Task<IdentityUser?> AttachRoleToUser(string roleName, string userId);

    public Task<IdentityUser?> RevokeRoleFromUser(string roleName, string userId);
}