using AuctionAPI_10_Api.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    [HttpPost]
    public void Post([FromBody] CategoryRequest categoryRequest)
    {
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] CategoryRequest categoryRequest)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}