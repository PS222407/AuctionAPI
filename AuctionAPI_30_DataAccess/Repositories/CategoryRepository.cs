﻿using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;

namespace AuctionAPI_30_DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _dbContext;

    public CategoryRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Category> GetAll()
    {
        return _dbContext.Categories.ToList();
    }

    public bool Create(Category category)
    {
        _dbContext.Categories.Add(category);
        return _dbContext.SaveChanges() > 0;
    }

    public Category? GetById(long id)
    {
        return _dbContext.Categories.FirstOrDefault(p => p.Id == id);
    }

    public bool Update(Category category)
    {
        _dbContext.Categories.Update(category);
        return _dbContext.SaveChanges() > 0;
    }

    public bool Delete(long id)
    {
        Category? category = _dbContext.Categories.FirstOrDefault(p => p.Id == id);
        if (category == null)
        {
            return false;
        }

        _dbContext.Categories.Remove(category);
        return _dbContext.SaveChanges() > 0;
    }
}