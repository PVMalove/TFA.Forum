using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Domain.EntityIds;

public record ForumId
{
    public Guid Id { get; }

    private static readonly IGuidFactory guidFactory = new GuidFactory();
    
    private ForumId(Guid id) => Id = id;

    public static ForumId NewId() => new(guidFactory.Create());
    public static ForumId Create(Guid id) => new(id);
    public static implicit operator Guid(ForumId forumId) => forumId.Id;
}