using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Domain.EntityIds;

public record UserId
{
    public Guid Id { get; }
    
    private static readonly IGuidFactory guidFactory = new GuidFactory();
    private UserId(Guid id) => Id = id;

    public static UserId NewId() => new(guidFactory.Create());
    public static UserId Create(Guid id) => new(id);
    public static implicit operator Guid(UserId userId) => userId.Id;
}