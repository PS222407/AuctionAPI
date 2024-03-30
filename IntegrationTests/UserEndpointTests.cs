using System.Net;

namespace IntegrationTests;

public class UserEndpointTests : IntegrationTestFixture
{
    [Test]
    public async Task GetAuctions_ShouldReturn401()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/User/Auctions/Won");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}