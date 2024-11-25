namespace TFA.Forum.Persistence.Storage.User;

public interface ISignOnStorage
{
    Task<Guid> CreateUser(string login, byte[] salt, byte[] hash, CancellationToken cancellationToken);
}