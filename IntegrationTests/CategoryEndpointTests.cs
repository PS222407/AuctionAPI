using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTests;

public class CategoryEndpointTests : IntegrationTestFixture
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

    [Test]
    public async Task PostCategory_ShouldReturn201()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Category", JsonContent.Create(new
        {
            Name = "Test Category",
            Icon = "Test Icon",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task PostCategory_InvalidBody_ShouldReturn400()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Category", JsonContent.Create(new
        {
            Name = "Test Category",
            Icon = "",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task PutCategory_ShouldReturn204()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PutAsync("api/v1/Category/1", JsonContent.Create(new
        {
            Name = "Test Category Changed",
            Icon = "Test Icon Changed",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task PutCategory_NonExistingCategory_ShouldReturn404()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PutAsync("api/v1/Category/100", JsonContent.Create(new
        {
            Name = "Test Category Changed",
            Icon = "Test Icon Changed",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task DeleteCategory_ShouldReturn204()
    {
        HttpResponseMessage httpResponseMessage = await Client!.DeleteAsync("api/v1/Category/1");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task DeleteCategory_NonExistingCategory_ShouldReturn404()
    {
        HttpResponseMessage httpResponseMessage = await Client!.DeleteAsync("api/v1/Category/100");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}