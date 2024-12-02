using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Domain.EntityIds;

public record ForumId
{
    public Guid Id { get; }
    
    private ForumId(Guid id) => Id = id;

    public static ForumId NewId(IGuidFactory guidFactory) => new(guidFactory.Create());
    public static ForumId Create(Guid id) => new(id);
    public static implicit operator Guid(ForumId forumId) => forumId.Id;
}