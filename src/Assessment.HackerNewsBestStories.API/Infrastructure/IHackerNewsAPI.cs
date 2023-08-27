namespace Assessment.HackerNewsBestStories.API.Infrastructure;

public interface IHackerNewsAPI {
    [Get("/v0/beststories.json")]
    Task<IEnumerable<int>> GetBestStories();

    [Get("/v0/item/{itemID}.json")]
    Task<HackerNewsItem> GetItem(int itemID);
}
