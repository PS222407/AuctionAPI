using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AuctionAPI_10_Api.RequestModels;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public IEnumerable<RoleViewModel> Get()
    {
        return _roleService.GetAll().Select(x => new RoleViewModel
        {
            Name = x.Name
        });
    }

    [HttpPost("attachRoleToUser")]
    public async Task AttachRoleToUser([FromBody] UserRoleRequest userRoleRequest)
    {
        await _roleService.AttachRoleToUser(userRoleRequest.RoleName, userRoleRequest.UserId);
    }

    [HttpPost("revokeRoleFromUser")]
    public async Task RevokeRoleFromUser(UserRoleRequest userRoleRequest)
    {
        await _roleService.RevokeRoleFromUser(userRoleRequest.RoleName, userRoleRequest.UserId);
    }
}
