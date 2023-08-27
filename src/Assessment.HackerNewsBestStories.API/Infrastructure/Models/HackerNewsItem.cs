namespace Assessment.HackerNewsBestStories.API.Infrastructure.Models;

public record HackerNewsItem(
    int ID,
    bool? Deleted,
    string? Type,
    string? By,
    long Time,
    string? Text,
    bool? Dead,
    int? Parent,
    int? Poll,
    int[]? Kids,
    string? URL,
    int? Score,
    string? Title,
    int[]? Parts,
    int? Descendants
);
