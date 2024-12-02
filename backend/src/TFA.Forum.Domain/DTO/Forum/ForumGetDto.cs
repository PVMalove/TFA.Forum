namespace TFA.Forum.Domain.DTO.Forum;

public record ForumGetDto(Guid Id, string? Title, DateTimeOffset CreatedAt);