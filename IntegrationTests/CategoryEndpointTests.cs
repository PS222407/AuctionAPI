using System.Net;

namespace IntegrationTests;

public class CategoryEndpointTests : IntegrationTestFixture
{
    [Test]
    public async Task GetCategories_ShouldReturn200()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Category");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetCategoryById_ShouldReturn200()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Category/1");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetCategoryById_IdDoesNotExist_ShouldReturn404()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Category/100");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}