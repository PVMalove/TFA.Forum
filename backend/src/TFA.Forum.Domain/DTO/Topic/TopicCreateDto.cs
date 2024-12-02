namespace TFA.Forum.Domain.DTO.Topic;

public record TopicCreateDto(Guid Id, Guid forumId, Guid authorId, string? title,
    string? content, DateTimeOffset CreatedAt);