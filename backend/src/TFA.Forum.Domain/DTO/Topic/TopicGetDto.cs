namespace TFA.Forum.Domain.DTO.Topic;

public record TopicGetDto(Guid Id, Guid forumId, Guid authorId, string? title,
    string? content, DateTimeOffset CreatedAt);