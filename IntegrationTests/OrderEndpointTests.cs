using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTests;

public class OrderEndpointTests : IntegrationTestFixture
{
    private string _accessToken = null!;

    [SetUp]
    public new async Task Setup()
    {
        await base.Setup();

        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Login", JsonContent.Create(new
        {
            Email = "admin@gmail.com",
            Password = "Password123!",
        }));
        _accessToken = (await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponse>())!.AccessToken;

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
    }

    [Test]
    public async Task GetOrders_ShouldReturn200()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Order");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetOrderById_ShouldReturn200()
    {
        const string orderId = "11d3105a-a235-4856-87e6-85da6f2b0860";
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync($"api/v1/Order/{orderId}");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetOrderById_ShouldReturn404()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Order/100");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}