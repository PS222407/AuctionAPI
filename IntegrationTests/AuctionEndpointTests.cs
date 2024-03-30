using System.Net;

namespace IntegrationTests;

public class Tests : IntegrationTestFixture
{
    [Test]
    public async Task GetAuctions_ShouldReturn200()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Auction");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetAuctionById_ShouldReturn200()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Auction/1");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetAuctionById_IdDoesNotExist_ShouldReturn404()
    {
        HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Auction/100");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}