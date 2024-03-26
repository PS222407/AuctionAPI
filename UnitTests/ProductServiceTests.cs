using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_20_BusinessLogic.Services;
using Moq;

namespace UnitTests;

[TestFixture]
public class ProductServiceTests
{
    private readonly ProductService _productService;

    private readonly Mock<IProductRepository> _productRepositoryMock = new();

    public ProductServiceTests()
    {
        _productService = new ProductService(_productRepositoryMock.Object);
    }

    [Test]
    public void GetAll_ShouldReturnAllProducts()
    {
        // Arrange
        List<Product> products =
        [
            new Product { Id = 1, Name = "Product 1" },
            new Product { Id = 2, Name = "Product 2" },
        ];
        _productRepositoryMock.Setup(x => x.Get())
            .Returns(products);

        // Act
        List<Product> result = _productService.Get();

        // Assert
        Assert.That(result, Is.EqualTo(products));
    }

    [Test]
    public void Create_ShouldCreateProduct()
    {
        // Arrange
        Product product = new() { Name = "Product 1" };
        _productRepositoryMock.Setup(x => x.Create(product))
            .Returns(new Product());

        // Act
        Product? result = _productService.Create(product);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetById_ShouldReturnProduct()
    {
        // Arrange
        Product product = new() { Id = 1, Name = "Product 1" };
        _productRepositoryMock.Setup(x => x.GetById(1))
            .Returns(product);

        // Act
        Product? result = _productService.GetById(1);

        // Assert
        Assert.That(result, Is.EqualTo(product));
    }

    [Test]
    public void Update_ShouldUpdateProduct()
    {
        // Arrange
        Product product = new() { Id = 1, Name = "Product 1" };
        _productRepositoryMock.Setup(x => x.Update(product))
            .Returns(true);

        // Act
        bool result = _productService.Update(product);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Delete_ShouldDeleteProduct()
    {
        // Arrange
        _productRepositoryMock.Setup(x => x.Delete(1))
            .Returns(true);

        // Act
        bool result = _productService.Delete(1);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Exists_ShouldReturnTrue()
    {
        // Arrange
        _productRepositoryMock.Setup(x => x.Exists(1))
            .Returns(true);

        // Act
        bool result = _productService.Exists(1);

        // Assert
        Assert.That(result, Is.True);
    }
}