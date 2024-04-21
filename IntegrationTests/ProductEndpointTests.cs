using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;

namespace IntegrationTests;

public class ProductEndpointTests : IntegrationTestFixture
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
    
    [Test]
    public async Task PostProduct_ShouldReturn201()
    {
        string currentDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!.Replace(
                Path.Combine("bin", "Debug", "net8.0"), "");
        string imagePath = Path.Combine(currentDirectory, "zwizuudvpwp31.jpg");
        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
        
        ByteArrayContent byteArrayContent = new(imageBytes);
        byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        
        MultipartFormDataContent formData = new();
        formData.Add(byteArrayContent, "Image", "zwizuudvpwp31.jpg");
        formData.Add(new StringContent("Product name"), "Name");
        formData.Add(new StringContent("1070"), "PriceInCents");
        formData.Add(new StringContent("Product description"), "Description");
        formData.Add(new StringContent("1"), "CategoryId");
        
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Product", formData);
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
    
    [Test]
    public async Task PostProduct_InvalidBody_ShouldReturn400()
    {
        string currentDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!.Replace(
                Path.Combine("bin", "Debug", "net8.0"), "");
        string imagePath = Path.Combine(currentDirectory, "zwizuudvpwp31.jpg");
        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
        
        ByteArrayContent byteArrayContent = new(imageBytes);
        byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        
        MultipartFormDataContent formData = new();
        formData.Add(byteArrayContent, "Image", "zwizuudvpwp31.jpg");
        formData.Add(new StringContent("Product name"), "Name");
        formData.Add(new StringContent("1"), "CategoryId");
        
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Product", formData);
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test]
    public async Task PostProduct_CategoryNotFound_ShouldReturn400()
    {
        string currentDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!.Replace(
                Path.Combine("bin", "Debug", "net8.0"), "");
        string imagePath = Path.Combine(currentDirectory, "zwizuudvpwp31.jpg");
        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
        
        ByteArrayContent byteArrayContent = new(imageBytes);
        byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        
        MultipartFormDataContent formData = new();
        formData.Add(byteArrayContent, "Image", "zwizuudvpwp31.jpg");
        formData.Add(new StringContent("Product name"), "Name");
        formData.Add(new StringContent("Product description"), "Description");
        formData.Add(new StringContent("100"), "CategoryId");
        
        HttpResponseMessage httpResponseMessage = await Client!.PostAsync("api/v1/Product", formData);
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test]
    public async Task PutProduct_ShouldReturn204()
    {
        string currentDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!.Replace(
                Path.Combine("bin", "Debug", "net8.0"), "");
        string imagePath = Path.Combine(currentDirectory, "zwizuudvpwp31.jpg");
        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
        
        ByteArrayContent byteArrayContent = new(imageBytes);
        byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        
        MultipartFormDataContent formData = new();
        formData.Add(byteArrayContent, "Image", "zwizuudvpwp31.jpg");
        formData.Add(new StringContent("Product name Changed"), "Name");
        formData.Add(new StringContent("1070"), "PriceInCents");
        formData.Add(new StringContent("Product description Changed"), "Description");
        formData.Add(new StringContent("2"), "CategoryId");
        
        HttpResponseMessage httpResponseMessage = await Client!.PutAsync("api/v1/Product/1", formData);
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
    
    [Test]
    public async Task PutProduct_ProductDoesNotExist_ShouldReturn404()
    {
        string currentDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!.Replace(
                Path.Combine("bin", "Debug", "net8.0"), "");
        string imagePath = Path.Combine(currentDirectory, "zwizuudvpwp31.jpg");
        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
        
        ByteArrayContent byteArrayContent = new(imageBytes);
        byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        
        MultipartFormDataContent formData = new();
        formData.Add(byteArrayContent, "Image", "zwizuudvpwp31.jpg");
        formData.Add(new StringContent("Product name Changed"), "Name");
        formData.Add(new StringContent("Product description Changed"), "Description");
        formData.Add(new StringContent("2"), "CategoryId");
        
        HttpResponseMessage httpResponseMessage = await Client!.PutAsync("api/v1/Product/100", formData);
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    [Test]
    public async Task PutProduct_InvalidBody_ShouldReturn400()
    {
        string currentDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!.Replace(
                Path.Combine("bin", "Debug", "net8.0"), "");
        string imagePath = Path.Combine(currentDirectory, "zwizuudvpwp31.jpg");
        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
        
        ByteArrayContent byteArrayContent = new(imageBytes);
        byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        
        MultipartFormDataContent formData = new();
        formData.Add(byteArrayContent, "Image", "zwizuudvpwp31.jpg");
        formData.Add(new StringContent("Product description Changed"), "Description");
        formData.Add(new StringContent("1"), "CategoryId");
        
        HttpResponseMessage httpResponseMessage = await Client!.PutAsync("api/v1/Product/1", formData);
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test]
    public async Task PutProduct_CategoryNotFound_ShouldReturn400()
    {
        string currentDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!.Replace(
                Path.Combine("bin", "Debug", "net8.0"), "");
        string imagePath = Path.Combine(currentDirectory, "zwizuudvpwp31.jpg");
        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
        
        ByteArrayContent byteArrayContent = new(imageBytes);
        byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        
        MultipartFormDataContent formData = new();
        formData.Add(byteArrayContent, "Image", "zwizuudvpwp31.jpg");
        formData.Add(new StringContent("Product name Changed"), "Name");
        formData.Add(new StringContent("Product description Changed"), "Description");
        formData.Add(new StringContent("100"), "CategoryId");
        
        HttpResponseMessage httpResponseMessage = await Client!.PutAsync("api/v1/Product/1", formData);
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test]
    public async Task DeleteProduct_ShouldReturn204()
    {
        HttpResponseMessage httpResponseMessage = await Client!.DeleteAsync("api/v1/Product/1");
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
    
    [Test]
    public async Task DeleteProduct_ProductDoesNotExist_ShouldReturn404()
    {
        HttpResponseMessage httpResponseMessage = await Client!.DeleteAsync("api/v1/Product/100");
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}