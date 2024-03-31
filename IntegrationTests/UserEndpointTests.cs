// using System.Net;
// using System.Net.Http.Headers;
// using System.Net.Http.Json;
//
// namespace IntegrationTests;
//
// public class UserEndpointTests : IntegrationTestFixture
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
//         HttpResponseMessage httpResponseMessage = await Client!.GetAsync("api/v1/User/Auctions/Won");
//
//         Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//     }
// }

