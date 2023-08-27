namespace Assessment.HackerNewsBestStories.API.Application.DTOs;

public record StoryDTO(
    string Title,
    Uri URI,
    string PostedBy,
    DateTime Time,
    uint Score,
    uint CommentCount
);
