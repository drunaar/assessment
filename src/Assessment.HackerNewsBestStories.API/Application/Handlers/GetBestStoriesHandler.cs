using Polly.Registry;

namespace Assessment.HackerNewsBestStories.API.Application.Handlers;

public class GetBestStoriesHandler : IRequestHandler<GetBestStoriesQuery, StoryDTO[]> {
    public const string HackerNewsCachePolicyKey = nameof(HackerNewsCachePolicyKey);
    private readonly IAsyncPolicy<HackerNewsItem>? _cachePolicy;
    private readonly IHackerNewsAPI _hackerNews;

    public GetBestStoriesHandler(IReadOnlyPolicyRegistry<string> policies, IHackerNewsAPI hackerNews) {
        _hackerNews = hackerNews;
        policies.TryGet(HackerNewsCachePolicyKey, out _cachePolicy);
    }

    public async Task<StoryDTO[]> Handle(GetBestStoriesQuery request, CancellationToken cancellationToken) {
        var bestStoriesIDs = await _hackerNews.GetBestStories();

        var retrievalTasks = bestStoriesIDs.Select(async id => await (
            _cachePolicy?.ExecuteAsync(async context => await _hackerNews.GetItem(id), new Context($"HackerNewsItem_{id}"))
            ?? _hackerNews.GetItem(id)));
        var bestStoriesItems = await Task.WhenAll(retrievalTasks.ToArray());

        return await Task.FromResult(bestStoriesItems
            .OrderByDescending(i => i.Score ?? 0)
            .Take((int)request.Top)
            .Select(item => item.Adapt<StoryDTO>())
            .ToArray());
    }
}
