using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Domain.EntityIds;

public record SessionId
{
    public Guid Id { get; }
    
    private static readonly IGuidFactory guidFactory = new GuidFactory();
    
    private SessionId(Guid id) => Id = id;

    public static SessionId NewId() => new(guidFactory.Create());
    public static SessionId Create(Guid id) => new(id);
    public static implicit operator Guid(SessionId sessionId) => sessionId.Id;  
    
}