namespace Assessment.HackerNewsBestStories.API.Application.Handlers;

public class GetBestStoriesHandler : IRequestHandler<GetBestStoriesQuery, StoryDTO[]> {
    public Task<StoryDTO[]> Handle(GetBestStoriesQuery request, CancellationToken cancellationToken) {
        return Task.FromResult(Enumerable.Repeat(
                new StoryDTO("foo", new Uri("http://foo.com"), "foo_person", DateTime.Now, 268, 1785),
                request.Top > 200 ? 200 : (int)request.Top)
            .ToArray());
    }
}
