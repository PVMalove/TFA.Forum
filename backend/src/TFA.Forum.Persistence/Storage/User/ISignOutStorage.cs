namespace TFA.Forum.Persistence.Storage.User;

public interface ISignOutStorage
{
    Task RemoveSession(Guid sessionId, CancellationToken cancellationToken);
}