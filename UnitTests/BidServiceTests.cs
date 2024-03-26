using AuctionAPI_20_BusinessLogic.Exceptions;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_20_BusinessLogic.Services;
using Moq;

namespace UnitTests;

public class BidServiceTests
{
    private readonly Mock<IAuctionRepository> _auctionRepositoryMock = new();

    private readonly Mock<IBidRepository> _bidRepositoryMock = new();

    private readonly BidService _bidService;

    public BidServiceTests()
    {
        AuctionService auctionService = new(_auctionRepositoryMock.Object);
        _bidService = new BidService(_bidRepositoryMock.Object, auctionService);
    }

    [Test]
    public void Create_ShouldReturnTrue()
    {
        // Arrange
        Bid bid = new() { AuctionId = 1, PriceInCents = 1000 };
        Auction auction = new()
        {
            Id = 1, StartDateTime = DateTime.UtcNow, DurationInSeconds = 7200, Bids =
            [
                new Bid { PriceInCents = 150 },
                new Bid { PriceInCents = 300 },
            ],
        };
        _auctionRepositoryMock.Setup(x => x.GetById(1))
            .Returns(auction);
        _bidRepositoryMock.Setup(x => x.Create(bid))
            .Returns(true);

        // Act
        bool result = _bidService.Create(bid);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Create_ShouldThrowAuctionNotAvailableException()
    {
        // Arrange
        Bid bid = new() { AuctionId = 1, PriceInCents = 1000 };
        Auction auction = new()
        {
            Id = 1, StartDateTime = DateTime.UtcNow, DurationInSeconds = 600, Bids =
            [
                new Bid { PriceInCents = 150 },
                new Bid { PriceInCents = 300 },
            ],
        };
        _auctionRepositoryMock.Setup(x => x.GetById(1))
            .Returns(auction);
        _bidRepositoryMock.Setup(x => x.Create(bid))
            .Returns(true);

        // Act
        // Assert
        Assert.Throws<AuctionNotAvailableException>(() => _bidService.Create(bid));
    }

    [Test]
    public void Create_ShouldThrowBidTooLowException()
    {
        // Arrange
        Bid bid = new() { AuctionId = 1, PriceInCents = 100 };
        Auction auction = new()
        {
            Id = 1, StartDateTime = DateTime.UtcNow, DurationInSeconds = 7200, Bids =
            [
                new Bid { PriceInCents = 150 },
                new Bid { PriceInCents = 300 },
            ],
        };
        _auctionRepositoryMock.Setup(x => x.GetById(1))
            .Returns(auction);
        _bidRepositoryMock.Setup(x => x.Create(bid))
            .Returns(true);

        // Act
        // Assert
        Assert.Throws<BidTooLowException>(() => _bidService.Create(bid));
    }
}