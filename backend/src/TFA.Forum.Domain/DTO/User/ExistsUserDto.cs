namespace TFA.Forum.Domain.DTO.User;

public record ExistsUserDto(Guid UserId, byte[] Salt, byte[] PasswordHash);