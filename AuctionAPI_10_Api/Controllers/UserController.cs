using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{email}")]
    public IEnumerable<UserViewModel> Get(string email)
    {
        return _userService.SearchByEmail(email).Select(x => new UserViewModel
        {
            Id = x.Id,
            Name = x.UserName,
            Email = x.Email
        });
    }
}