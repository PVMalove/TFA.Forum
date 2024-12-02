using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Domain.EntityIds;

public record TopicId
{
    public Guid Id { get; }
    
    private TopicId(Guid id) => Id = id;

    public static TopicId NewId(IGuidFactory guidFactory) => new(guidFactory.Create());
    public static TopicId Create(Guid id) => new(id);
    public static implicit operator Guid(TopicId topicId) => topicId.Id;
}