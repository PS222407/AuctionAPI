using AuctionAPI_20_BusinessLogic;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_30_DataAccess.Data;

namespace AuctionAPI_30_DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _dbContext;

    public ProductRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Product> GetAll()
    {
        return _dbContext.Products.ToList();
    }

    public bool Create(Product product)
    {
        _dbContext.Products.Add(product);
        return _dbContext.SaveChanges() > 0;
    }

    public Product? GetById(int id)
    {
        return _dbContext.Products.Find(id);
    }

    public bool Update(Product product)
    {
        _dbContext.Products.Update(product);
        return _dbContext.SaveChanges() > 0;
    }

    public bool Delete(int id)
    {
        Product? product = _dbContext.Products.Find(id);
        if (product == null)
        {
            return false;
        }

        _dbContext.Products.Remove(product);
        return _dbContext.SaveChanges() > 0;
    }
}