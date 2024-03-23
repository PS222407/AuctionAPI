using AuctionAPI_10_Api.RequestModels;
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
public class CategoryController(
    ICategoryService categoryService,
    IConfiguration configuration,
    IValidator<CategoryRequest> validator) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(categoryService.Get().Select(c => new CategoryViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Icon = c.Icon,
        }));
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        Category? category = categoryService.GetById(id);
        if (category == null)
        {
            return NotFound();
        }

        CategoryViewModel categoryViewModel = new()
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon,
            Products = category.Products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImageUrl = p.ImageIsExternal ? p.ImageUrl : $"{configuration["BackendUrl"]}{p.ImageUrl}",
            }).ToList(),
        };

        return Ok(categoryViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Post([FromBody] CategoryRequest categoryRequest)
    {
        ValidationResult result = validator.Validate(categoryRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        Category category = new()
        {
            Name = categoryRequest.Name,
            Icon = categoryRequest.Icon,
        };

        categoryService.Create(category);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] CategoryRequest categoryRequest)
    {
        if (!categoryService.Exists(id))
        {
            return NotFound();
        }

        ValidationResult result = validator.Validate(categoryRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        Category category = new()
        {
            Id = id,
            Name = categoryRequest.Name,
            Icon = categoryRequest.Icon,
        };

        categoryService.Update(category);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!categoryService.Exists(id))
        {
            return NotFound();
        }

        categoryService.Delete(id);

        return NoContent();
    }
}