using TFA.Forum.Domain.DTO.Topic;

namespace TFA.Forum.Persistence.Storage.Topic;

public interface ICreateTopicStorage
{
    Task<TopicCreateDto> CreateTopic(Guid forumId, Guid authorId, string? title,
        string? content, CancellationToken cancellationToken);
}