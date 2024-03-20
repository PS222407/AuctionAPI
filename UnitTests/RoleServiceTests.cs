using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace UnitTests;

[TestFixture]
public class RoleServiceTests
{
    private readonly RoleService _roleService;

    private readonly Mock<IRoleRepository> _roleRepositoryMock = new();

    public RoleServiceTests()
    {
        _roleService = new RoleService(_roleRepositoryMock.Object);
    }

    [Test]
    public void GetAll_ShouldReturnAllRoles()
    {
        // Arrange
        List<IdentityRole> roles =
        [
            new IdentityRole { Name = "Role 1" },
            new IdentityRole { Name = "Role 2" },
        ];
        _roleRepositoryMock.Setup(x => x.Get())
            .Returns(roles);

        // Act
        IEnumerable<IdentityRole> result = _roleService.Get();

        // Assert
        Assert.That(result, Is.EqualTo(roles));
    }

    [Test]
    public void AttachRoleToUser_ShouldAttachRoleToUser()
    {
        // Arrange
        IdentityUser user = new() { Id = "1" };
        _roleRepositoryMock.Setup(x => x.AttachRoleToUser("Role", "1"))
            .ReturnsAsync(user);

        // Act
        IdentityUser result = _roleService.AttachRoleToUser("Role", "1").Result;

        // Assert
        Assert.That(result, Is.EqualTo(user));
    }

    [Test]
    public void RevokeRoleFromUser_ShouldRevokeRoleFromUser()
    {
        // Arrange
        IdentityUser user = new() { Id = "1" };
        _roleRepositoryMock.Setup(x => x.RevokeRoleFromUser("Role", "1"))
            .ReturnsAsync(user);

        // Act
        IdentityUser result = _roleService.RevokeRoleFromUser("Role", "1").Result;

        // Assert
        Assert.That(result, Is.EqualTo(user));
    }
}