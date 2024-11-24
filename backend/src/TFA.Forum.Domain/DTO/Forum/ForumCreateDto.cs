namespace TFA.Forum.Domain.DTO.Forum;

public record ForumCreateDto(Guid Id, string? Title, DateTimeOffset CreatedAt);