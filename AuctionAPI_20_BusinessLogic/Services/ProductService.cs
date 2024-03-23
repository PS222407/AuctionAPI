using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public List<Product> Get()
    {
        return productRepository.Get();
    }

    public bool Create(Product product)
    {
        return productRepository.Create(product);
    }

    public Product? GetById(long id)
    {
        return productRepository.GetById(id);
    }

    public bool Update(Product product)
    {
        return productRepository.Update(product);
    }

    public bool Delete(long id)
    {
        return productRepository.Delete(id);
    }

    public bool Exists(long id)
    {
        return productRepository.Exists(id);
    }
}