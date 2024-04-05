using AuctionAPI_20_BusinessLogic.Helpers;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_20_BusinessLogic.Services;
using Mollie.Api.Models.Payment.Response;
using Moq;

namespace UnitTests;

public class OrderServiceTests
{
    private readonly Mock<IMollieHelper> _mollieHelperMock = new();

    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();

    private OrderService _orderService = null!;

    [SetUp]
    public void Setup()
    {
        _orderService = new OrderService(_orderRepositoryMock.Object, _mollieHelperMock.Object);
    }

    [Test]
    public void Create_ShouldReturnOrder()
    {
        // Act
        const string userId = "F91178CE-FFCE-40ED-955F-3471BC6A0586";
        const string paymentId = "F91178CE-FFCE-40ED-955F-3471BC6A0586";
        const string orderId = "F91178CE-FFCE-40ED-955F-3471BC6A0586";
        Product product = new()
        {
            Id = 1,
            PriceInCents = 1070,
        };
        Order order = new()
        {
            Id = orderId,
            ProductId = 1,
            UserId = userId,
            PriceInCents = 1070,
        };
        _orderRepositoryMock.Setup(x => x.Create(It.IsAny<Order>()))
            .Returns(order);

        // Arrange
        Order result = _orderService.Create(product, userId, paymentId, orderId);

        // Assert
        Assert.That(result, Is.EqualTo(order));
    }

    [Test]
    public void GetBy_ShouldReturnOrder()
    {
        // Arrange
        const string orderId = "F91178CE-FFCE-40ED-955F-3471BC6A0586";
        const string userId = "77C10784-D645-406F-869D-C653B19948F5";
        Order order = new()
        {
            Id = orderId,
            UserId = userId,
            PriceInCents = 1259,
        };
        _orderRepositoryMock.Setup(x => x.GetBy(orderId))
            .Returns(order);

        // Act
        Order? result = _orderService.GetBy(orderId);

        // Assert
        Assert.That(result, Is.EqualTo(order));
    }

    [Test]
    public void Update_ShouldReturnTrue()
    {
        // Arrange
        Order order = new()
        {
            Id = "F91178CE-FFCE-40ED-955F-3471BC6A0586",
            UserId = "F91178CE-FFCE-40ED-955F-3471BC6A0586",
            PriceInCents = 1259,
        };
        _orderRepositoryMock.Setup(x => x.Update(order))
            .Returns(true);

        // Act
        bool result = _orderService.Update(order);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task CreateMolliePayment_ShouldReturnPaymentResponse()
    {
        // Arrange
        const int totalAmount = 1259;
        const string orderId = "F91178CE-FFCE-40ED-955F-3471BC6A0586";
        PaymentResponse expectedResponse = new();

        _mollieHelperMock.Setup(client => client.CreatePayment(totalAmount, orderId))
            .ReturnsAsync(expectedResponse);

        // Act
        PaymentResponse result = await _orderService.CreateMolliePayment(totalAmount, orderId);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task GetMolliePayment_ShouldReturnPaymentResponse()
    {
        // Arrange
        const string externalPaymentId = "A54E9D75-50E0-4245-B0A9-A557B2DE5C07";
        PaymentResponse expectedResponse = new();

        _mollieHelperMock.Setup(client => client.GetPayment(externalPaymentId))
            .ReturnsAsync(expectedResponse);

        // Act
        PaymentResponse result = await _orderService.GetPaymentFromMollie(externalPaymentId);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetByExternalPaymentId_ShouldReturnPaymentResponse()
    {
        // Arrange
        const string externalPaymentId = "A54E9D75-50E0-4245-B0A9-A557B2DE5C07";
        const string userId = "A54E9D75-50E0-4245-B0A9-A557B2DE5C07";
        const string orderId = "A54E9D75-50E0-4245-B0A9-A557B2DE5C07";
        Order order = new()
        {
            Id = orderId,
            UserId = userId,
            PriceInCents = 1259,
        };

        _orderRepositoryMock.Setup(x => x.GetByExternalPaymentId(externalPaymentId))
            .Returns(order);

        // Act
        Order? result = _orderService.GetByExternalPaymentId(externalPaymentId);

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}