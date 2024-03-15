using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IProductRepository
{
    public List<Product> GetAll();
    
    public bool Create(Product product);
    
    public Product? GetById(long id);
    
    public bool Update(Product product);
    
    public bool Delete(long id);
}