using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.Services;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    public IValidator<ProductCreateRequest> UpdateValidator { get; }

    private readonly ICategoryService _categoryService;

    private readonly IConfiguration _configuration;

    private readonly FileService _fileService;

    private readonly IProductService _productService;

    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IValidator<ProductCreateRequest> _createValidator;
    
    private readonly IValidator<ProductUpdateRequest> _updateValidator;

    public ProductController(
        FileService fileService,
        IProductService productService,
        IWebHostEnvironment webHostEnvironment,
        IConfiguration configuration,
        ICategoryService categoryService, 
        IValidator<ProductCreateRequest> validator, IValidator<ProductCreateRequest> createValidator, IValidator<ProductCreateRequest> updateValidator, IValidator<ProductUpdateRequest> updateValidator1)
    {
        UpdateValidator = updateValidator;
        _updateValidator = updateValidator1;
        _productService = productService;
        _fileService = fileService;
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
        _categoryService = categoryService;
        _createValidator = createValidator;
    }

    [HttpGet]
    public IEnumerable<ProductViewModel> Get()
    {
        return _productService.Get().Select(x => new ProductViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            ImageUrl = x.ImageIsExternal ? x.ImageUrl : $"{_configuration["BackendUrl"]}{x.ImageUrl}",
        });
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(long id)
    {
        Product? product = _productService.GetById(id);
        if (product == null)
        {
            return NotFound();
        }

        ProductViewModel productViewModel = new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageIsExternal ? product.ImageUrl : $"{_configuration["BackendUrl"]}{product.ImageUrl}",
            Category = product.Category == null
                ? null
                : new CategoryViewModel
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name,
                },
            Auctions = product.Auctions.Select(a => new AuctionViewModel
            {
                Id = a.Id,
                StartDateTime = a.StartDateTime,
                DurationInSeconds = a.DurationInSeconds,
            }).ToList(),
        };

        return Ok(productViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Post([FromForm] ProductCreateRequest productCreateRequest)
    {
        ValidationResult result = await _createValidator.ValidateAsync(productCreateRequest);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        if (!_categoryService.Exists(productCreateRequest.CategoryId ?? 0))
        {
            return BadRequest(new { Message = "Category not found" });
        }

        string imageUrl;
        try
        {
            imageUrl = await _fileService.SaveImageAsync(productCreateRequest.Image, _webHostEnvironment) ?? "";
        }
        catch (FileFormatException)
        {
            return BadRequest(new { Errors = new { Image = new List<string> { "File is not an image" } } });
        }

        Product product = new()
        {
            Name = productCreateRequest.Name,
            Description = productCreateRequest.Description,
            ImageUrl = imageUrl,
            CategoryId = productCreateRequest.CategoryId ?? 0,
        };

        _productService.Create(product);

        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Put(int id, [FromForm] ProductUpdateRequest productUpdateRequest)
    {
        Product? product = _productService.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        ValidationResult result = await _updateValidator.ValidateAsync(productUpdateRequest);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }
        
        if (!_categoryService.Exists(productUpdateRequest.CategoryId ?? 0))
        {
            return NotFound(new { Message = "Category not found" });
        }

        string imageUrl = product.ImageUrl;
        try
        {
            if (productUpdateRequest.Image != null)
            {
                imageUrl = await _fileService.SaveImageAsync(productUpdateRequest.Image, _webHostEnvironment) ?? "";
            }
        }
        catch (FileFormatException)
        {
            return BadRequest(new { Errors = new { Image = new List<string> { "File is not an image" } } });
        }

        product.Name = productUpdateRequest.Name;
        product.Description = productUpdateRequest.Description;
        product.ImageUrl = imageUrl;
        product.CategoryId = productUpdateRequest.CategoryId ?? 0;
        product.ImageIsExternal = productUpdateRequest.Image == null ? product.ImageIsExternal : false;

        _productService.Update(product);

        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!_productService.Exists(id))
        {
            return NotFound();
        }
        
        _productService.Delete(id);
        
        return NoContent();
    }
}