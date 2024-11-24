using TFA.Forum.Domain.DTO.User;

namespace TFA.Forum.Persistence.Storage.User;

public interface ISignInStorage
{
    Task<RecognisedUserDto?> FindUser(string login, CancellationToken cancellationToken);
}