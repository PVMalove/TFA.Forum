namespace TFA.Forum.Domain.DTO.User;

public record RecognisedUserDto(Guid UserId, byte[] Salt, byte[] PasswordHash);