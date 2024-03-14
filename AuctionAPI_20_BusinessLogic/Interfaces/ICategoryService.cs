using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface ICategoryService
{
    public List<Category> GetAll();
    
    public bool Create(Category product);
    
    public Category? GetById(int id);
    
    public bool Update(Category product);
    
    public bool Delete(int id);
}