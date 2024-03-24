using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public List<Category> Get()
    {
        return categoryRepository.Get();
    }

    public Category? Create(Category category)
    {
        return categoryRepository.Create(category);
    }

    public Category? GetById(long id)
    {
        return categoryRepository.GetById(id);
    }

    public bool Update(Category category)
    {
        return categoryRepository.Update(category);
    }

    public bool Delete(long id)
    {
        return categoryRepository.Delete(id);
    }

    public bool Exists(long id)
    {
        return categoryRepository.Exists(id);
    }
}