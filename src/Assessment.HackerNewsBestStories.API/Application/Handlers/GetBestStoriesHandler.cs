namespace Assessment.HackerNewsBestStories.API.Application.Handlers;

public class GetBestStoriesHandler : IRequestHandler<GetBestStoriesQuery, StoryDTO[]> {
    private readonly IHackerNewsAPI _hackerNews;

    public GetBestStoriesHandler(IHackerNewsAPI hackerNews)
        => _hackerNews = hackerNews;

    public async Task<StoryDTO[]> Handle(GetBestStoriesQuery request, CancellationToken cancellationToken) {
        var bestStoriesIDs = await _hackerNews.GetBestStories();

        var retrievalTasks = bestStoriesIDs.Select(async id => await _hackerNews.GetItem(id));
        var bestStoriesItems = await Task.WhenAll(retrievalTasks.ToArray());

        return await Task.FromResult(bestStoriesItems
            .OrderByDescending(i => i.Score ?? 0)
            .Take((int)request.Top)
            .Select(item => item.Adapt<StoryDTO>())
            .ToArray());
    }
}
