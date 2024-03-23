using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    // private readonly IRoleService _roleService;
    //
    // private readonly SignInManager<IdentityUser> _signInManager;
    //
    // public RoleController(IRoleService roleService, SignInManager<IdentityUser> signInManager)
    // {
    //     _roleService = roleService;
    //     _signInManager = signInManager;
    // }
    //
    // [HttpGet]
    // public IActionResult Get()
    // {
    //     return Ok(_roleService.Get().Select(x => new RoleViewModel
    //     {
    //         Name = x.Name,
    //     }));
    // }
    //
    // [HttpPost("attachRoleToUser")]
    // public async Task<IActionResult> AttachRoleToUser([FromBody] UserRoleRequest userRoleRequest)
    // {
    //     IdentityUser? user = await _roleService.AttachRoleToUser(userRoleRequest.RoleName, userRoleRequest.UserId);
    //     if (user == null)
    //     {
    //         return NotFound(new { Message = "User not found" });
    //     }
    //
    //     await _signInManager.RefreshSignInAsync(user);
    //
    //     return NoContent();
    // }
    //
    // [HttpPost("revokeRoleFromUser")]
    // public async Task<IActionResult> RevokeRoleFromUser(UserRoleRequest userRoleRequest)
    // {
    //     IdentityUser? user = await _roleService.RevokeRoleFromUser(userRoleRequest.RoleName, userRoleRequest.UserId);
    //     if (user != null)
    //     {
    //         await _signInManager.RefreshSignInAsync(user);
    //     }
    //
    //     return NoContent();
    // }
}