using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_30_DataAccess.Repositories;

public class ProductRepository(DataContext dbContext) : IProductRepository
{
    public List<Product> Get()
    {
        return dbContext.Products.ToList();
    }
    
    public Product? Create(Product product)
    {
        dbContext.Products.Add(product);
        return dbContext.SaveChanges() <= 0 ? null : product;
    }
    
    public Product? GetById(long id)
    {
        DateTime now = DateTime.Now;
        
        return dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Auctions.Where(a => a.StartDateTime.AddSeconds(a.DurationInSeconds) > now).OrderBy(a => a.StartDateTime))
            .FirstOrDefault(p => p.Id == id);
    }
    
    public bool Update(Product product)
    {
        dbContext.Products.Update(product);
        return dbContext.SaveChanges() > 0;
    }
    
    public bool Delete(long id)
    {
        Product? product = dbContext.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return false;
        }
        
        dbContext.Products.Remove(product);
        return dbContext.SaveChanges() > 0;
    }
    
    public bool Exists(long id)
    {
        return dbContext.Products.Any(p => p.Id == id);
    }
}