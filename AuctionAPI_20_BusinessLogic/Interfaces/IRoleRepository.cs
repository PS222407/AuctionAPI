using Microsoft.AspNetCore.Identity;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IRoleRepository
{
    public IEnumerable<IdentityRole> GetAll();

    public Task AttachRoleToUser(string roleName, string userId);

    public Task RevokeRoleFromUser(string roleName, string userId);
}