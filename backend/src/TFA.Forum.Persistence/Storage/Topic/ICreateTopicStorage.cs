namespace TFA.Forum.Persistence.Storage.Topic;

public interface ICreateTopicStorage
{
    Task<Domain.Entities.Topic> CreateTopic(Guid forumId, Guid userId, string? title, string? content,
        CancellationToken cancellationToken);
}