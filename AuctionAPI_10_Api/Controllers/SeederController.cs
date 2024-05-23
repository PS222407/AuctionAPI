using AuctionAPI_30_DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SeederController(DataContext dbContext, IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(204)]
    public async Task<ActionResult> Seed()
    {
        string? connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        
        Log.Information("{@connectionString}", connectionString);
        
        await new SeedData(dbContext).ResetDatabaseAndSeed();
        
        return NoContent();
    }
}