using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<Product> Get()
    {
        return _productRepository.Get();
    }
    
    public bool Create(Product product)
    {

        
        return _productRepository.Create(product);
    }

    public Product? GetById(long id)
    {
        return _productRepository.GetById(id);
    }
    
    public bool Update(Product product)
    {
        return _productRepository.Update(product);
    }
    
    public bool Delete(long id)
    {
        return _productRepository.Delete(id);
    }
}