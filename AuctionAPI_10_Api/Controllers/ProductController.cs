using AuctionAPI_10_Api.Mappers;
using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.Services;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController(
    IProductService productService,
    IWebHostEnvironment webHostEnvironment,
    IConfiguration configuration,
    ICategoryService categoryService,
    IValidator<ProductCreateRequest> createValidator,
    IValidator<ProductUpdateRequest> updateValidator)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    public IActionResult Get()
    {
        return Ok(productService.Get().Select(x => ProductMapper.MapToViewModel(x, configuration)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult Get(long id)
    {
        Product? product = productService.GetById(id);
        if (product == null)
        {
            return NotFound();
        }

        ProductViewModel productViewModel = ProductMapper.MapToViewModel(product, configuration);

        return Ok(productViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromForm] ProductCreateRequest productCreateRequest)
    {
        ValidationResult result = await createValidator.ValidateAsync(productCreateRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        if (!categoryService.Exists(productCreateRequest.CategoryId ?? 0))
        {
            return BadRequest(new { Message = "Category not found" });
        }

        Product product = new()
        {
            Name = productCreateRequest.Name,
            Description = productCreateRequest.Description,
            ImageUrl = await FileService.SaveImageAsync(productCreateRequest.Image, webHostEnvironment) ?? "",
            CategoryId = productCreateRequest.CategoryId ?? 0,
        };

        Product? createdProduct = productService.Create(product);
        if (createdProduct == null)
        {
            return BadRequest(new { Message = "Product could not be created" });
        }

        return CreatedAtAction("Get", new { id = product.Id },
            ProductMapper.MapToViewModel(createdProduct, configuration));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Put(int id, [FromForm] ProductUpdateRequest productUpdateRequest)
    {
        Product? product = productService.GetById(id);
        if (product == null)
        {
            return NotFound();
        }

        ValidationResult result = await updateValidator.ValidateAsync(productUpdateRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        if (!categoryService.Exists(productUpdateRequest.CategoryId ?? 0))
        {
            return NotFound(new { Message = "Category not found" });
        }

        string imageUrl = product.ImageUrl;
        if (productUpdateRequest.Image != null)
        {
            imageUrl = await FileService.SaveImageAsync(productUpdateRequest.Image, webHostEnvironment) ?? "";
        }

        product.Name = productUpdateRequest.Name;
        product.Description = productUpdateRequest.Description;
        product.ImageUrl = imageUrl;
        product.CategoryId = productUpdateRequest.CategoryId ?? 0;
        product.ImageIsExternal = productUpdateRequest.Image == null ? product.ImageIsExternal : false;

        productService.Update(product);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult Delete(int id)
    {
        if (!productService.Exists(id))
        {
            return NotFound();
        }

        productService.Delete(id);

        return NoContent();
    }
}