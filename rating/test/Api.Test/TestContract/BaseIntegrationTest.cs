namespace Api.Test.TestContract;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient _httpClient;

    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _httpClient = factory.CreateDefaultClient();
    }
}