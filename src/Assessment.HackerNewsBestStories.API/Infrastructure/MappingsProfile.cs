namespace Assessment.HackerNewsBestStories.API.Infrastructure;

public class MappingsProfile : IRegister {
    public void Register(TypeAdapterConfig config) {
        config.ForType<HackerNewsItem, StoryDTO>()
            /*
            .Map(dst => dst.Time, src => DateTimeOffset.FromUnixTimeSeconds(src.Time).UtcDateTime)
            .Map(dst => dst.URI, src => new Uri(src.URL ?? string.Empty))
            .Map(dst => dst.PostedBy, src => src.By)
            .Map(dst => dst.CommentCount, src => src.Descendants)
            */
            // Workaround for https://github.com/MapsterMapper/Mapster/issues/623
            .MapWith(src => new StoryDTO(
                src.Title ?? string.Empty,
                Uri.IsWellFormedUriString(src.URL, UriKind.RelativeOrAbsolute)
                    ? new Uri(src.URL)
                    : new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                src.By ?? string.Empty,
                DateTimeOffset.FromUnixTimeSeconds(src.Time).UtcDateTime,
                (uint)(src.Score ?? 0),
                (uint)(src.Descendants ?? 0)));
    }
}
