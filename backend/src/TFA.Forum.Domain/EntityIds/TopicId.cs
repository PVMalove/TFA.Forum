using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Domain.EntityIds;

public record TopicId
{
    public Guid Id { get; }
    
    private static readonly IGuidFactory guidFactory = new GuidFactory();
    private TopicId(Guid id) => Id = id;

    public static TopicId NewId() => new(guidFactory.Create());
    public static TopicId Create(Guid id) => new(id);
    public static implicit operator Guid(TopicId topicId) => topicId.Id;
}