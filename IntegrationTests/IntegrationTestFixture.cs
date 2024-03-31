using AuctionAPI_30_DataAccess.Data;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

public abstract class IntegrationTestFixture : IDisposable
{
    private IntegrationTestWebApplicationFactory? _factory;

    protected HttpClient? Client;

    public void Dispose()
    {
        Client = _factory?.CreateClient();
    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new IntegrationTestWebApplicationFactory();
    }

    [SetUp]
    public async Task Setup()
    {
        Client = _factory!.CreateClient();
        using IServiceScope scope = _factory.Services.CreateScope();

        DataContext dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        await new SeedData(dbContext).ResetDatabaseAndSeed();
    }
}