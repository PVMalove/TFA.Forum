using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.DTO.User;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Storage.User;

public class AuthenticationStorage : IAuthenticationStorage
{
    private readonly IBaseRepository<Session> sessionRepository;

    public AuthenticationStorage(IBaseRepository<Session> sessionRepository)
    {
        this.sessionRepository = sessionRepository;
    }

    public async Task<SessionDto?> FindSession(Guid sessionId, CancellationToken cancellationToken)
    {
        var result = await sessionRepository.GetAll()
            .Where(s => s.Id == sessionId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        
        return new SessionDto(result.UserId, result.ExpiresAt);
    }
}