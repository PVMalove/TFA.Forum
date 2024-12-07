using TFA.Forum.Domain.EntityIds;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Storage.User;

internal class SignOnStorage(
    IBaseRepository<Domain.Entities.User> userRepository)
    : ISignOnStorage
{
    public async Task<Guid> CreateUser(string login, byte[] salt, byte[] hash, CancellationToken cancellationToken)
    {
        var userIdValue = UserId.NewId();
        
        var user = Domain.Entities.User.Create(userIdValue, login, salt, hash);

        await userRepository.Create(user, cancellationToken);
        await userRepository.SaveChanges(cancellationToken);

        return userIdValue;
    }
}