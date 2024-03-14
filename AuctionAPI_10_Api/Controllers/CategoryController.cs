using AuctionAPI_10_Api.RequestModels;
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
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet]
    public IEnumerable<CategoryViewModel> Get()
    {
        return _categoryService.GetAll().Select(x => new CategoryViewModel
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    [HttpGet("{id}")]
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
            Name = category.Name
        };
        
        return categoryViewModel;
    }

    [HttpPost]
    public void Post([FromBody] CategoryRequest categoryRequest)
    {
        Category category = new()
        {
            Name = categoryRequest.Name
        };
        
        _categoryService.Create(category);
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] CategoryRequest categoryRequest)
    {
        Category category = new()
        {
            Id = id,
            Name = categoryRequest.Name
        };
        
        _categoryService.Update(category);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        _categoryService.Delete(id);
    }
}