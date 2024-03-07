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

    public IEnumerable<IdentityRole> GetAll()
    {
       return _roleRepository.GetAll();
    }

    public async Task AttachRoleToUser(string roleName, string userId)
    {
        await _roleRepository.AttachRoleToUser(roleName, userId);
    }

    public async Task RevokeRoleFromUser(string roleName, string userId)
    {
        await _roleRepository.RevokeRoleFromUser(roleName, userId);
    }
}