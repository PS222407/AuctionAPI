using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_30_DataAccess.Repositories;

public class CategoryRepository(DataContext dbContext) : ICategoryRepository
{
    public List<Category> Get()
    {
        return dbContext.Categories.ToList();
    }

    public Category? Create(Category category)
    {
        dbContext.Categories.Add(category);
        return dbContext.SaveChanges() <= 0 ? null : category;
    }

    public Category? GetById(long id)
    {
        return dbContext.Categories
            .Include(c => c.Products)
            .FirstOrDefault(c => c.Id == id);
    }

    public bool Update(Category category)
    {
        dbContext.Categories.Update(category);
        return dbContext.SaveChanges() > 0;
    }

    public bool Delete(long id)
    {
        Category? category = dbContext.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return false;
        }

        dbContext.Categories.Remove(category);
        return dbContext.SaveChanges() > 0;
    }

    public bool Exists(long id)
    {
        return dbContext.Categories.Any(c => c.Id == id);
    }
}