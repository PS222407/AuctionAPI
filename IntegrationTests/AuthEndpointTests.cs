using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTests;

public class AuthEndpointTests : IntegrationTestFixture
{
    private string _accessToken = null!;

    private string _refreshToken = null!;

    [SetUp]
    public new async Task Setup()
    {
        await base.Setup();

        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Login", JsonContent.Create(new
        {
            Email = "admin@gmail.com",
            Password = "Password123!",
        }));
        LoginResponse loginResponse = (await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponse>())!;
        _accessToken = loginResponse.AccessToken;
        _refreshToken = loginResponse.RefreshToken;

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
    }

    [Test]
    public async Task Register_ShouldReturn200()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Register", JsonContent.Create(new
        {
            Email = "test@gmail.com",
            Password = "Password123!",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Register_InvalidBody_ShouldReturn400()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Register", JsonContent.Create(new
        {
            Password = "Password123!",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Login_ShouldReturn200()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Login", JsonContent.Create(new
        {
            Email = "admin@gmail.com",
            Password = "Password123!",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Login_InvalidBody_ShouldReturn400()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Login", JsonContent.Create(new
        {
            Password = "Password123!",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Login_UserDoesNotExist_ShouldReturn401()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Login", JsonContent.Create(new
        {
            Email = "arminvanbuuren@gmail.com",
            Password = "Password123!",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Login_WrongPassword_ShouldReturn401()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Login", JsonContent.Create(new
        {
            Email = "admin@gmail.com",
            Password = "arminvanbuuren",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Refresh_ShouldReturn200()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Refresh", JsonContent.Create(new
        {
            RefreshToken = _refreshToken,
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Refresh_InvalidBody_ShouldReturn400()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Refresh", JsonContent.Create(new
        {
            RefreshToken = "",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Refresh_InvalidToken_ShouldReturn401()
    {
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Refresh", JsonContent.Create(new
        {
            RefreshToken = _refreshToken + "123",
        }));

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}