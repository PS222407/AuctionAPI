using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    private readonly IConfiguration _configuration;

    public CategoryController(ICategoryService categoryService, IConfiguration configuration)
    {
        _categoryService = categoryService;
        _configuration = configuration;
    }

    [HttpGet]
    public IEnumerable<CategoryViewModel> Get()
    {
        return _categoryService.Get().Select(c => new CategoryViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Icon = c.Icon,
        });
    }

    [HttpGet("{id:int}")]
    public CategoryViewModel? Get(int id)
    {
        Category? category = _categoryService.GetById(id);
        if (category == null)
        {
            return null;
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
                ImageUrl = p.ImageIsExternal ? p.ImageUrl : $"{_configuration["BackendUrl"]}{p.ImageUrl}",
            }).ToList(),
        };

        return categoryViewModel;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public void Post([FromBody] CategoryRequest categoryRequest)
    {
        Category category = new()
        {
            Name = categoryRequest.Name,
            Icon = categoryRequest.Icon,
        };

        _categoryService.Create(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public void Put(int id, [FromBody] CategoryRequest categoryRequest)
    {
        Category category = new()
        {
            Id = id,
            Name = categoryRequest.Name,
            Icon = categoryRequest.Icon,
        };

        _categoryService.Update(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public void Delete(int id)
    {
        _categoryService.Delete(id);
    }
}