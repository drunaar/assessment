using System.Text.Json.Nodes;

namespace Assessment.HackerNewsBestStories.FunctionalTests;

public class BasicEndpointScenarios : IClassFixture<WebAppFactory> {
    private readonly WebAppFactory _factory;

    public BasicEndpointScenarios(WebAppFactory factory)
        => _factory = factory;

    [Fact]
    public async void Default_request_is_successful() {
        var client = _factory.CreateDefaultClient();
        var response = await client.GetAsync("/beststories");

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async void Response_contains_valid_document() {
        var client = _factory.CreateDefaultClient();
        var response = await client.GetStringAsync("/beststories?top=2");
        var document = JsonNode.Parse(response)?.AsArray();

        Assert.NotNull(document);
        Assert.Equal(2, document.Count);
        var item = document[0] as JsonObject;
        Assert.NotNull(item);
        Assert.Collection(item,
            n => Assert.Equal("title", n.Key),
            n => Assert.Equal("uri", n.Key),
            n => Assert.Equal("postedBy", n.Key),
            n => {
                Assert.Equal("time", n.Key);
                Assert.True(DateTime.TryParseExact(n.Value?.ToString(), "yyyy-MM-ddTHH:mm:ssK", null, DateTimeStyles.None, out var _));
            },
            n => {
                Assert.Equal("score", n.Key);
                Assert.True(int.TryParse(n.Value?.ToString(), out var _));
            },
            n => {
                Assert.Equal("commentCount", n.Key);
                Assert.True(int.TryParse(n.Value?.ToString(), out var _));
            });
    }

    [Fact]
    public async void Request_with_negative_top_filter_returns_BadRequest() {
        var client = _factory.CreateDefaultClient();
        var response = await client.GetAsync("/beststories?top=-1");

        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(99)]
    [InlineData(156)]
    [InlineData(200)]
    public async void Response_contains_requested_number_of_records(int topFilterValue) {
        var client = _factory.CreateDefaultClient();
        var response = await client.GetStringAsync($"/beststories?top={topFilterValue}");

        Assert.Equal(topFilterValue, JsonNode.Parse(response)?.AsArray().Count);
    }

    [Fact]
    public async void Response_contains_up_to_200_records() {
        var client = _factory.CreateDefaultClient();
        var response = await client.GetStringAsync("/beststories?top=250");

        Assert.Equal(200, JsonNode.Parse(response)?.AsArray().Count);
    }
}
