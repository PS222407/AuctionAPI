using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.Services;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Authorize(Roles = "Admin")]
[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    private readonly FileService _fileService;

    private readonly IWebHostEnvironment _webHostEnvironment;
    
    private readonly IConfiguration _configuration;

    public ProductController(
        FileService fileService,
        IProductService productService,
        IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        _productService = productService;
        _fileService = fileService;
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
    }

    [HttpGet]
    public IEnumerable<ProductViewModel> Get()
    {
        return _productService.GetAll().Select(x => new ProductViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            ImageUrl = $"{_configuration["BackendUrl"]}{x.ImageUrl}"
        });
    }

    [HttpGet("{id:int}")]
    public ProductViewModel? Get(int id)
    {
        Product? product = _productService.GetById(id);
        if (product == null)
        {
            return null;
        }

        ProductViewModel productViewModel = new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
        };

        return productViewModel;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task Post([FromForm] ProductRequest productRequest)
    {
        Product product = new()
        {
            Name = productRequest.Name,
            Description = productRequest.Description,
            ImageUrl = await _fileService.SaveImageAsync(productRequest.Image, _webHostEnvironment) ?? "",
        };
        
        _productService.Create(product);
    }

    [HttpPut("{id:int}")]
    public void Put(int id, [FromBody] ProductRequest productRequest)
    {
        Product product = new()
        {
            Id = id,
            Name = productRequest.Name
        };

        _productService.Update(product);
    }

    [HttpDelete("{id:int}")]
    public void Delete(int id)
    {
        _productService.Delete(id);
    }
}