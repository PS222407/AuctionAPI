using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IProductService
{
    public List<Product> Get();
    
    public bool Create(Product product);
    
    public Product? GetById(long id);
    
    public bool Update(Product product);
    
    public bool Delete(long id);
    
    public bool Exists(long id);
}