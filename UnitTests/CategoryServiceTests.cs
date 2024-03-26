using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_20_BusinessLogic.Services;
using Moq;

namespace UnitTests;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();

    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _categoryService = new CategoryService(_categoryRepositoryMock.Object);
    }

    [Test]
    public void GetAll_ShouldReturnAllCategories()
    {
        // Arrange
        List<Category> categories =
        [
            new Category { Id = 1 },
            new Category { Id = 2 },
        ];
        _categoryRepositoryMock.Setup(x => x.Get())
            .Returns(categories);

        // Act
        List<Category> result = _categoryService.Get();

        // Assert
        Assert.That(result, Is.EqualTo(categories));
    }

    [Test]
    public void Create_ShouldCreateCategory()
    {
        // Arrange
        Category category = new() { Id = 1 };
        _categoryRepositoryMock.Setup(x => x.Create(category))
            .Returns(new Category());

        // Act
        Category? result = _categoryService.Create(category);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetById_ShouldReturnCategory()
    {
        // Arrange
        Category category = new() { Id = 1 };
        _categoryRepositoryMock.Setup(x => x.GetById(1))
            .Returns(category);

        // Act
        Category? result = _categoryService.GetById(1);

        // Assert
        Assert.That(result, Is.EqualTo(category));
    }

    [Test]
    public void Update_ShouldUpdateCategory()
    {
        // Arrange
        Category category = new() { Id = 1 };
        _categoryRepositoryMock.Setup(x => x.Update(category))
            .Returns(true);

        // Act
        bool result = _categoryService.Update(category);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Delete_ShouldDeleteCategory()
    {
        // Arrange
        _categoryRepositoryMock.Setup(x => x.Delete(1))
            .Returns(true);

        // Act
        bool result = _categoryService.Delete(1);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Exists_ShouldReturnTrue()
    {
        // Arrange
        _categoryRepositoryMock.Setup(x => x.Exists(1))
            .Returns(true);

        // Act
        bool result = _categoryService.Exists(1);

        // Assert
        Assert.That(result, Is.True);
    }
}