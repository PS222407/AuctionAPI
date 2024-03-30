using System.Net;

namespace IntegrationTests;

public class ProductEndpointTests : IntegrationTestFixture
{
    [Test]
    public async Task GetProducts_ShouldReturn200()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Product");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetProductById_ShouldReturn200()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Product/1");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetProductById_IdDoesNotExist_ShouldReturn404()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Product/100");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}