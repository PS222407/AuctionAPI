using AuctionAPI_20_BusinessLogic.DataModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_20_BusinessLogic.Services;
using FluentAssertions;
using Moq;

namespace UnitTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Test]
    public void GetWonAuctions()
    {
        // Arrange
        string userId = "002ECDB1-7A75-4178-923F-89CA75D8F766";
        DateTime now = DateTime.Now;
        List<WonAuctionsDataModel> wonAuctions =
        [
            new WonAuctionsDataModel
            {
                a__id = 1,
                a__ProductId = 1,
                a__StartDateTime = now,
                a__DurationInSeconds = 7200,
                p__Id = 1,
                p__Name = "Product 1",
                p__Description = "Product 1 Description",
                p__ImageUrl = "https://example.com/image.jpg",
                p__CategoryId = 1,
                p__ImageIsExternal = true,
                b__Id = 1,
                b__AuctionId = 1,
                b__PriceInCents = 1000,
                b__CreatedAt = now,
                b__UserId = userId,
            },
            new WonAuctionsDataModel
            {
                a__id = 2,
                a__ProductId = 2,
                a__StartDateTime = now,
                a__DurationInSeconds = 10800,
                p__Id = 2,
                p__Name = "Product 2",
                p__Description = "Product 2 Description",
                p__ImageUrl = "https://example.com/image.jpg",
                p__CategoryId = 2,
                p__ImageIsExternal = true,
                b__Id = 2,
                b__AuctionId = 2,
                b__PriceInCents = 1590,
                b__CreatedAt = now,
                b__UserId = userId,
            },
        ];
        List<Auction> auctions =
        [
            new Auction
            {
                Id = 1,
                ProductId = 1,
                StartDateTime = now,
                DurationInSeconds = 7200,
                Product = new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "Product 1 Description",
                    ImageUrl = "https://example.com/image.jpg",
                    CategoryId = 1,
                    ImageIsExternal = true,
                },
                Bids =
                [
                    new Bid
                    {
                        Id = 1,
                        AuctionId = 1,
                        PriceInCents = 1000,
                        CreatedAt = now,
                        UserId = userId,
                    },
                ],
            },
            new Auction
            {
                Id = 2,
                ProductId = 2,
                StartDateTime = now,
                DurationInSeconds = 10800,
                Product = new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "Product 2 Description",
                    ImageUrl = "https://example.com/image.jpg",
                    CategoryId = 2,
                    ImageIsExternal = true,
                },
                Bids =
                [
                    new Bid
                    {
                        Id = 2,
                        AuctionId = 2,
                        PriceInCents = 1590,
                        CreatedAt = now,
                        UserId = userId,
                    },
                ],
            },
        ];
        _userRepositoryMock.Setup(x => x.GetWonAuctions(userId))
            .Returns(wonAuctions);

        // Act
        List<Auction> result = _userService.GetWonAuctions(userId);

        // Assert
        result.Should().BeEquivalentTo(auctions);
    }
}