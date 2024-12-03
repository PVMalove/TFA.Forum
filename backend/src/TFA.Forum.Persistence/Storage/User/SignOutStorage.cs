using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Storage.User;

internal class SignOutStorage : ISignOutStorage
{
    private readonly IBaseRepository<Session> sessionRepository;

    public SignOutStorage(IBaseRepository<Session> sessionRepository)
    {
        this.sessionRepository = sessionRepository;
    }

    public async Task RemoveSession(Guid sessionId, CancellationToken cancellationToken)
    {
        var session = await sessionRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);
        
        if (session != null) 
            sessionRepository.Remove(session);
            
        await sessionRepository.SaveChanges(cancellationToken);
    }
}