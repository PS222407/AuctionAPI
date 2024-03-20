using AuctionAPI_30_DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SeederController : ControllerBase
{
    private readonly DataContext _dbContext;

    public SeederController(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Seed()
    {
        await new SeedData(_dbContext).ResetDatabaseAndSeed();

        return Ok();
    }
}