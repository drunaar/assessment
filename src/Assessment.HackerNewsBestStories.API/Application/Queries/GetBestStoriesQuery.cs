namespace Assessment.HackerNewsBestStories.API.Application.Queries;

public record GetBestStoriesQuery : IRequest<StoryDTO[]> {
    public GetBestStoriesQuery(uint top)
        => Top = top;

    public uint Top { get; init; }
}
