using AuctionAPI_20_BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuctionAPI_20_BusinessLogic.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public IEnumerable<IdentityRole> Get()
    {
        return _roleRepository.Get();
    }

    public async Task<IdentityUser?> AttachRoleToUser(string roleName, string userId)
    {
        return await _roleRepository.AttachRoleToUser(roleName, userId);
    }

    public async Task<IdentityUser?> RevokeRoleFromUser(string roleName, string userId)
    {
        return await _roleRepository.RevokeRoleFromUser(roleName, userId);
    }
}