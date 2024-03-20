using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SeederController : ControllerBase
{
    [HttpGet]
    public IActionResult Seed()
    {
        return Ok();
    }
}