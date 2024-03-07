using AuctionAPI_20_BusinessLogic.Interfaces;

namespace AuctionAPI_20_BusinessLogic.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<Product> GetAll()
    {
        return _productRepository.GetAll();
    }
    
    public bool Create(Product product)
    {
        return _productRepository.Create(product);
    }

    public Product? GetById(int id)
    {
        return _productRepository.GetById(id);
    }
    
    public bool Update(Product product)
    {
        return _productRepository.Update(product);
    }
    
    public bool Delete(int id)
    {
        return _productRepository.Delete(id);
    }
}