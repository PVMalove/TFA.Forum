namespace TFA.Forum.Domain.DTO.User;

public record SessionDto(Guid UserId, DateTimeOffset ExpiresAt);