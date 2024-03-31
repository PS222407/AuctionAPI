using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_20_BusinessLogic.Services;
using Moq;

namespace UnitTests;

public class AuctionServiceTests
{
    private readonly Mock<IAuctionRepository> _auctionRepositoryMock = new();

    private readonly AuctionService _auctionService;

    public AuctionServiceTests()
    {
        _auctionService = new AuctionService(_auctionRepositoryMock.Object);
    }

    [Test]
    public void GetAll_ShouldReturnAllAuctions()
    {
        // Arrange
        List<Auction> auctions =
        [
            new Auction { Id = 1 },
            new Auction { Id = 2 },
        ];
        _auctionRepositoryMock.Setup(x => x.Get())
            .Returns(auctions);

        // Act
        List<Auction> result = _auctionService.Get();

        // Assert
        Assert.That(result, Is.EqualTo(auctions));
    }

    [Test]
    public void Create_ShouldCreateAuction()
    {
        // Arrange
        Auction auction = new() { Id = 1 };
        _auctionRepositoryMock.Setup(x => x.Create(auction))
            .Returns(new Auction());

        // Act
        Auction? result = _auctionService.Create(auction);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetById_ShouldReturnAuction()
    {
        // Arrange
        Auction auction = new() { Id = 1 };
        _auctionRepositoryMock.Setup(x => x.GetById(1))
            .Returns(auction);

        // Act
        Auction? result = _auctionService.GetById(1);

        // Assert
        Assert.That(result, Is.EqualTo(auction));
    }

    [Test]
    public void Update_ShouldUpdateAuction()
    {
        // Arrange
        Auction auction = new() { Id = 1 };
        _auctionRepositoryMock.Setup(x => x.Update(auction))
            .Returns(true);

        // Act
        bool result = _auctionService.Update(auction);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Delete_ShouldDeleteAuction()
    {
        // Arrange
        _auctionRepositoryMock.Setup(x => x.Delete(1))
            .Returns(true);

        // Act
        bool result = _auctionService.Delete(1);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsRunning_ShouldReturnTrue()
    {
        // Arrange
        Auction auction = new() { Id = 1, StartDateTime = DateTime.UtcNow, DurationInSeconds = 14400 };
        _auctionRepositoryMock.Setup(x => x.GetById(1))
            .Returns(auction);

        // Act
        bool result = _auctionService.IsRunning(1);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsRunning_ShouldReturnFalse()
    {
        // Arrange
        Auction auction = new() { Id = 1, StartDateTime = DateTime.UtcNow.AddSeconds(600), DurationInSeconds = 10 };
        _auctionRepositoryMock.Setup(x => x.GetById(1))
            .Returns(auction);

        // Act
        bool result = _auctionService.IsRunning(1);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsRunning_NullAuction_ShouldReturnFalse()
    {
        // Arrange
        _auctionRepositoryMock.Setup(x => x.GetById(1))
            .Returns((Auction)null!);

        // Act
        bool result = _auctionService.IsRunning(1);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Exists_ShouldReturnTrue()
    {
        // Arrange
        _auctionRepositoryMock.Setup(x => x.Exists(1))
            .Returns(true);

        // Act
        bool result = _auctionService.Exists(1);

        // Assert
        Assert.That(result, Is.True);
    }
}