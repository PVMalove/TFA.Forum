using TFA.Forum.Domain.Entities.Interfaces;
using TFA.Forum.Domain.EntityIds;

namespace TFA.Forum.Domain.Entities;

public class Session : Entity<SessionId>
{
    public User User { get; private set; }  = null!;
    public UserId UserId { get; init; } = null!;
    public DateTimeOffset ExpiresAt { get; init; }
    
    protected Session(SessionId id) : base(id) { }
    
    private Session(SessionId id, UserId userId, DateTimeOffset expiresAt) : base(id)
    {
        UserId = userId;
        ExpiresAt = expiresAt;
    }
    
    public static Session Create(SessionId id, UserId userId, DateTimeOffset expiresAt)
    { 
        return new Session(id,userId, expiresAt);
    }
}