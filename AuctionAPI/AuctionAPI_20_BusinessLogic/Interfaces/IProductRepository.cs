namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IProductRepository
{
    public List<Product> GetAll();
    
    public bool Create(Product product);
    
    public Product? GetById(int id);
    
    public bool Update(Product product);
    
    public bool Delete(int id);
}