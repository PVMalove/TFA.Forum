using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.DTO.User;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Storage.User;

internal class SignInStorage : ISignInStorage
{
    private readonly IBaseRepository<Domain.Entities.User> userRepository;

    public SignInStorage(IBaseRepository<Domain.Entities.User> userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<RecognisedUserDto?> FindUser(string login, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetAll()
            .AsNoTracking()
            .Where(u => u.Login.Equals(login))
            .FirstAsync(cancellationToken);
        
        return new RecognisedUserDto(result.Id, result.Salt, result.PasswordHash);
    }
}