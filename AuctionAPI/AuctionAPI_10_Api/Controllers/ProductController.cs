using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic;
using AuctionAPI_20_BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _ProductService;

    public ProductController(IProductService productService)
    {
        _ProductService = productService;
    }

    [HttpGet]
    public IEnumerable<ProductViewModel> Get()
    {
        return _ProductService.GetAll().Select(x => new ProductViewModel
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    [HttpGet("{id:int}")]
    public ProductViewModel? Get(int id)
    {
        Product? product = _ProductService.GetById(id);
        if (product == null)
        {
            return null;
        }

        ProductViewModel productViewModel = new()
        {
            Id = product.Id,
            Name = product.Name
        };

        return productViewModel;
    }

    [HttpPost]
    public void Post([FromBody] ProductRequest productRequest)
    {
        Product product = new()
        {
            Name = productRequest.Name
        };

        _ProductService.Create(product);
    }

    [HttpPut("{id:int}")]
    public void Put(int id, [FromBody] ProductRequest productRequest)
    {
        Product product = new()
        {
            Id = id,
            Name = productRequest.Name
        };

        _ProductService.Update(product);
    }

    [HttpDelete("{id:int}")]
    public void Delete(int id)
    {
        _ProductService.Delete(id);
    }
}