using AuctionAPI_10_Api.Mappers;
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
    [ProducesResponseType(200)]
    public ActionResult<List<CategoryViewModel>> Get()
    {
        return Ok(categoryService.Get().Select(c => CategoryMapper.MapToViewModel(c, configuration)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public ActionResult<CategoryViewModel> Get(int id)
    {
        Category? category = categoryService.GetById(id);
        if (category == null)
        {
            return NotFound();
        }

        CategoryViewModel categoryViewModel = CategoryMapper.MapToViewModel(category, configuration);

        return Ok(categoryViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public ActionResult<CategoryViewModel> Post([FromBody] CategoryRequest categoryRequest)
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

        Category? createdCategory = categoryService.Create(category);
        if (createdCategory == null)
        {
            return BadRequest(new { Message = "Category could not be created" });
        }

        return CreatedAtAction("Get", new { id = category.Id },
            CategoryMapper.MapToViewModel(createdCategory, configuration));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public ActionResult Put(int id, [FromBody] CategoryRequest categoryRequest)
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
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public ActionResult Delete(int id)
    {
        if (!categoryService.Exists(id))
        {
            return NotFound();
        }

        categoryService.Delete(id);

        return NoContent();
    }
}