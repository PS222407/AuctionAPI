using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface ICategoryRepository
{
    public List<Category> Get();

    public bool Create(Category product);

    public Category? GetById(long id);

    public bool Update(Category category);

    public bool Delete(long id);

    public bool Exists(long id);
}