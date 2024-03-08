using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace UnitTests;

[TestFixture]
public class UserServiceTests
{
    private readonly UserService _userService;

    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    public UserServiceTests()
    {
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Test]
    public void SearchByEmail_ShouldReturnUsers()
    {
        // Arrange
        List<IdentityUser> users =
        [
            new IdentityUser { Email = "test@test.test" },
        ];
        _userRepositoryMock.Setup(x => x.SearchByEmail("test"))
            .Returns(users);
        
        // Act
        List<IdentityUser> result = _userService.SearchByEmail("test");
        
        // Assert
        Assert.That(result, Is.EqualTo(users));
    }
}