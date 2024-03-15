using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/v1/[controller]")]
[ApiController]
public class AuctionController : ControllerBase
{
    private readonly IAuctionService _auctionService;
    
    private readonly IConfiguration _configuration;
    
    private readonly IProductService _productService;

    public AuctionController(IAuctionService auctionService, IConfiguration configuration, IProductService productService)
    {
        _auctionService = auctionService;
        _configuration = configuration;
        _productService = productService;
    }
    
    [HttpGet]
    public IEnumerable<AuctionViewModel> Get()
    {
        return _auctionService.Get().Select(a => new AuctionViewModel
        {
            Id = a.Id,
            Product = new ProductViewModel
            {
                Id = a.Product.Id,
                Name = a.Product.Name,
                Description = a.Product.Description,
                ImageUrl = $"{_configuration["BackendUrl"]}{a.Product.ImageUrl}"
            },
            DurationInSeconds = a.DurationInSeconds,
            StartDateTime = a.StartDateTime,
        });
    }
    
    [HttpGet("{id}")]
    public AuctionViewModel? Get(int id)
    {
        Auction? auction = _auctionService.GetById(id);
        if (auction == null)
        {
            return null;
        }
        
        AuctionViewModel auctionViewModel = new()
        {
            Id = auction.Id,
            Product = new ProductViewModel
            {
                Id = auction.Product.Id,
                Name = auction.Product.Name,
                Description = auction.Product.Description,
                ImageUrl = $"{_configuration["BackendUrl"]}{auction.Product.ImageUrl}"
            },
            DurationInSeconds = auction.DurationInSeconds,
            StartDateTime = auction.StartDateTime,
        };
        
        return auctionViewModel;
    }

    [HttpPost]
    public IActionResult Post([FromBody] AuctionRequest auctionRequest)
    {
        Product? product = _productService.GetById(auctionRequest.ProductId);
        if (product == null)
        {
            return BadRequest("Product not found");
        }
        
        Auction auction = new()
        {
            Product = product,
            DurationInSeconds = auctionRequest.DurationInSeconds,
            StartDateTime = auctionRequest.StartDateTime,
        };
        
        _auctionService.Create(auction);

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] AuctionRequest auctionRequest)
    {
        Product? product = _productService.GetById(auctionRequest.ProductId);
        if (product == null)
        {
            return BadRequest("Product not found");
        }
        
        Auction auction = new()
        {
            Id = id,
            Product = product,
            DurationInSeconds = auctionRequest.DurationInSeconds,
            StartDateTime = auctionRequest.StartDateTime,
        };
        
        _auctionService.Update(auction);

        return Ok();
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        _auctionService.Delete(id);
    }
}