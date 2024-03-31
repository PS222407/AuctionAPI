// using System.Net;
// using System.Net.Http.Headers;
// using System.Net.Http.Json;
//
// namespace IntegrationTests;
//
// public class BidEndpointTests : IntegrationTestFixture
// {
//     private string _accessToken = null!;
//
//     [SetUp]
//     public new async Task Setup()
//     {
//         await base.Setup();
//
//         HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/Login", JsonContent.Create(new
//         {
//             Email = "admin@gmail.com",
//             Password = "Password123!",
//         }));
//         _accessToken = (await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponse>())!.AccessToken;
//
//         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
//     }
//
//     [Test]
//     public async Task PostBid_ShouldReturn204()
//     {
//         // Arrange
//         HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Bid", JsonContent.Create(new
//         {
//             AuctionId = 4,
//             PriceInCents = 100,
//         }));
//
//         // Assert
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
//     }
//
//     [Test]
//     public async Task PostBid_IsNotRunning_ShouldReturn404()
//     {
//         // Arrange
//         HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Bid", JsonContent.Create(new
//         {
//             AuctionId = 1,
//             PriceInCents = 100,
//         }));
//
//         // Assert
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
//     }
//
//     [Test]
//     public async Task PostBid_InvalidBody_ShouldReturn400()
//     {
//         // Arrange
//         HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Bid", JsonContent.Create(new
//         {
//             AuctionId = 1,
//         }));
//
//         // Assert
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
//     }
// }

