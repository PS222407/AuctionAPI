﻿using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public List<Category> GetAll()
    {
        return _categoryRepository.GetAll();
    }
    
    public bool Create(Category category)
    {
        return _categoryRepository.Create(category);
    }

    public Category? GetById(int id)
    {
        return _categoryRepository.GetById(id);
    }
    
    public bool Update(Category category)
    {
        return _categoryRepository.Update(category);
    }
    
    public bool Delete(int id)
    {
        return _categoryRepository.Delete(id);
    }
}