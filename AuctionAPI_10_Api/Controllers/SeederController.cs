using AuctionAPI_30_DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SeederController(DataContext dbContext) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Seed()
    {
        await new SeedData(dbContext).ResetDatabaseAndSeed();

        return NoContent();
    }
}