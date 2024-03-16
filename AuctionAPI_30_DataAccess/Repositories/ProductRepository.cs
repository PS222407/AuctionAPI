using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_30_DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _dbContext;

    public ProductRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Product> Get()
    {
        return _dbContext.Products.ToList();
    }

    public bool Create(Product product)
    {
        _dbContext.Products.Add(product);
        return _dbContext.SaveChanges() > 0;
    }

    public Product? GetById(long id)
    {
        return _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Auctions)
            .FirstOrDefault(p => p.Id == id);
    }

    public bool Update(Product product)
    {
        _dbContext.Products.Update(product);
        return _dbContext.SaveChanges() > 0;
    }

    public bool Delete(long id)
    {
        Product? product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return false;
        }

        _dbContext.Products.Remove(product);
        return _dbContext.SaveChanges() > 0;
    }

    public bool Exists(long id)
    {
        return _dbContext.Products.Any(p => p.Id == id);
    }
}