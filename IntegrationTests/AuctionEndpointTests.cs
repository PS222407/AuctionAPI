// using System.Net;
// using System.Net.Http.Headers;
// using System.Net.Http.Json;
//
// namespace IntegrationTests;
//
// public class AuctionEndpointTests : IntegrationTestFixture
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
//     public async Task GetAuctions_ShouldReturn200()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Auction");
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//     }
//
//     [Test]
//     public async Task GetAuctionById_ShouldReturn200()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Auction/1");
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//     }
//
//     [Test]
//     public async Task GetAuctionById_IdDoesNotExist_ShouldReturn404()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/Auction/100");
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
//     }
//
//     [Test]
//     public async Task PostAuction_ShouldReturn201()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Auction", JsonContent.Create(new
//         {
//             ProductId = 1,
//             StartDateTime = DateTime.UtcNow,
//             DurationInSeconds = 3600,
//         }));
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
//     }
//
//     [Test]
//     public async Task PostAuction_InvalidBody_ShouldReturn400()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Auction", JsonContent.Create(new
//         {
//             ProductId = 1,
//             StartDateTime = DateTime.UtcNow,
//         }));
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
//     }
//
//     [Test]
//     public async Task PostAuction_ProductDoesNotExist_ShouldReturn400()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Auction", JsonContent.Create(new
//         {
//             ProductId = 100,
//             StartDateTime = DateTime.UtcNow,
//             DurationInSeconds = 3600,
//         }));
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
//     }
//
//     [Test]
//     public async Task PutAuction_ShouldReturn204()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.PutAsync("api/v1/Auction/1", JsonContent.Create(new
//         {
//             ProductId = 2,
//             StartDateTime = DateTime.UtcNow,
//             DurationInSeconds = 3601,
//         }));
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
//     }
//
//     [Test]
//     public async Task PutAuction_AuctionDoesNotExist_ShouldReturn404()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.PutAsync("api/v1/Auction/100", JsonContent.Create(new
//         {
//             ProductId = 1,
//             StartDateTime = DateTime.UtcNow,
//             DurationInSeconds = 3600,
//         }));
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
//     }
//
//     [Test]
//     public async Task PutAuction_InvalidBody_ShouldReturn400()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.PutAsync("api/v1/Auction/1", JsonContent.Create(new
//         {
//             ProductId = 100,
//             StartDateTime = DateTime.UtcNow,
//         }));
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
//     }
//
//     [Test]
//     public async Task PutAuction_ProductDoesNotExist_ShouldReturn400()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.PutAsync("api/v1/Auction/1", JsonContent.Create(new
//         {
//             ProductId = 100,
//             StartDateTime = DateTime.UtcNow,
//             DurationInSeconds = 3600,
//         }));
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
//     }
//
//     [Test]
//     public async Task DeleteAuction_ShouldReturn204()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.DeleteAsync("api/v1/Auction/1");
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
//     }
//
//     [Test]
//     public async Task DeleteAuction_ProductDoesNotExist_ShouldReturn404()
//     {
//         HttpResponseMessage httpResponseMessage = await Client!.DeleteAsync("api/v1/Auction/100");
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
//     }
// }

