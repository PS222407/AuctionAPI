using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.Services;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;

namespace AuctionAPI_10_Api.Controllers;

[Authorize(Roles = "Admin")]
[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    private readonly ICategoryService _categoryService;

    private readonly FileService _fileService;

    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IConfiguration _configuration;

    public ProductController(
        FileService fileService,
        IProductService productService,
        IWebHostEnvironment webHostEnvironment,
        IConfiguration configuration,
        ICategoryService categoryService
    )
    {
        _productService = productService;
        _fileService = fileService;
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
        _categoryService = categoryService;
    }

    [HttpGet]
    public IEnumerable<ProductViewModel> Get()
    {
        return _productService.Get().Select(x => new ProductViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            ImageUrl = $"{_configuration["BackendUrl"]}{x.ImageUrl}"
        });
    }

    [HttpGet("{id:int}")]
    public ProductViewModel? Get(long id)
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
            CategoryId = product.Category?.Id
        };

        return productViewModel;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Post([FromForm] ProductRequest productRequest)
    {
        if (!_categoryService.Exists(productRequest.CategoryId))
        {
            return NotFound(new { Message = "Category not found" });
        }

        string imageUrl;
        try
        {
            imageUrl = await _fileService.SaveImageAsync(productRequest.Image, _webHostEnvironment) ?? "";
        }
        catch (FileFormatException)
        {
            return BadRequest(new { Errors = new { Image = new List<string> { "File is not an image" } }});
        }

        Product product = new()
        {
            Name = productRequest.Name,
            Description = productRequest.Description,
            ImageUrl = imageUrl,
            CategoryId = productRequest.CategoryId
        };

        _productService.Create(product);

        return Ok();
    }

    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Put(int id, [FromForm] ProductRequest productRequest)
    {
        if (!_categoryService.Exists(productRequest.CategoryId))
        {
            return NotFound(new { Message = "Category not found" });
        }
        
        string imageUrl;
        try
        {
            imageUrl = await _fileService.SaveImageAsync(productRequest.Image, _webHostEnvironment) ?? "";
        }
        catch (FileFormatException)
        {
            return BadRequest(new { Errors = new { Image = new List<string> { "File is not an image" } }});
        }

        Product product = new()
        {
            Id = id,
            Name = productRequest.Name,
            Description = productRequest.Description,
            ImageUrl = imageUrl,
            CategoryId = productRequest.CategoryId
        };

        _productService.Update(product);

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public void Delete(int id)
    {
        _productService.Delete(id);
    }
}