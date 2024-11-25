using TFA.Forum.Domain.Interfaces;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Storage.User;

internal class SignOnStorage : ISignOnStorage
{
    private readonly IBaseRepository<Domain.Entities.User> userRepository;
    private readonly IGuidFactory guidFactory;

    public SignOnStorage(IBaseRepository<Domain.Entities.User> userRepository,
        IGuidFactory guidFactory)
    {
        this.userRepository = userRepository;
        this.guidFactory = guidFactory;
    }
    
    public async Task<Guid> CreateUser(string login, byte[] salt, byte[] hash, CancellationToken cancellationToken)
    {
        var userId = guidFactory.Create();

        var user = new Domain.Entities.User
        {
            Id = userId,
            Login = login,
            Salt = salt,
            PasswordHash = hash
        };

        await userRepository.Create(user, cancellationToken);
        await userRepository.SaveChanges(cancellationToken);

        return userId;
    }
}