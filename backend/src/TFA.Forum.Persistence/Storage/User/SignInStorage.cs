using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.DTO.User;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.EntityIds;
using TFA.Forum.Domain.Interfaces;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Storage.User;

internal class SignInStorage : ISignInStorage
{
    private readonly IBaseRepository<Domain.Entities.User> userRepository;
    private readonly IBaseRepository<Session> sessionRepository;

    private readonly IGuidFactory guidFactory;

    public SignInStorage(IBaseRepository<Domain.Entities.User> userRepository,
        IBaseRepository<Session> sessionRepository, 
        IGuidFactory guidFactory)
    {
        this.userRepository = userRepository;
        this.sessionRepository = sessionRepository;
        this.guidFactory = guidFactory;
    }

    public async Task<ExistsUserDto?> FindUserByLogin(string login, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetAll()
            .AsNoTracking()
            .Where(u => u.Login.Equals(login))
            .FirstAsync(cancellationToken);
        
        return new ExistsUserDto(result.Id, result.Salt, result.PasswordHash);
    }
    
    public async Task<Guid> CreateSession(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken)
    {
        var sessionIdValue = SessionId.NewId(guidFactory);
        var userIdValue = UserId.Create(userId);

        var session = Session.Create(sessionIdValue, userIdValue, expirationMoment);

        await sessionRepository.Create(session, cancellationToken);
        await sessionRepository.SaveChanges(cancellationToken);

        var result = await sessionRepository.GetAll()
            .AsNoTracking()
            .Where(t => t.Id == sessionIdValue)
            .FirstAsync(cancellationToken);
        
        return result.Id;
    }
}