namespace Assessment.HackerNewsBestStories.FunctionalTests;

public class BasicEndpointScenarios : IClassFixture<WebAppFactory> {
    private readonly HttpClient _httpClient;

    public BasicEndpointScenarios(WebAppFactory factory)
        => _httpClient = factory.CreateDefaultClient();

    [Fact]
    public async void Request_to_endpoint_is_successful() {
        var response = await _httpClient.GetAsync("/weatherforecast");

        Assert.True(response.IsSuccessStatusCode);
    }
}
