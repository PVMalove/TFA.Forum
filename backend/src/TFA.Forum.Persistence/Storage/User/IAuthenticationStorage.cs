using TFA.Forum.Domain.DTO.User;

namespace TFA.Forum.Persistence.Storage.User;

public interface IAuthenticationStorage
{
    Task<SessionDto> FindSession(Guid sessionId, CancellationToken cancellationToken);
}