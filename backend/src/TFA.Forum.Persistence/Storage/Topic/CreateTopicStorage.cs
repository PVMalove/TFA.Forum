using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.DTO.Topic;
using TFA.Forum.Domain.EntityIds;
using TFA.Forum.Domain.Interfaces;
using TFA.Forum.Domain.Interfaces.Repository;
using TFA.Forum.Domain.ValueObjects;

namespace TFA.Forum.Persistence.Storage.Topic;

public class CreateTopicStorage(
    IBaseRepository<Domain.Entities.Topic> topicRepository,
    IMomentProvider momentProvider)
    : ICreateTopicStorage
{
    public async Task<TopicCreateDto> CreateTopic(Guid forumId, Guid authorId, string? title,
        string? content, CancellationToken cancellationToken)
    {
        var topicIdValue = TopicId.NewId();
        var forumIdValue = ForumId.Create(forumId);
        var authorIdValue = UserId.Create(authorId);
        var titleValue = Title.Create(title).Value;
        var contentValue = Content.Create(content).Value;
        
        var topic = Domain.Entities.Topic.Create(topicIdValue, forumIdValue, authorIdValue, titleValue, contentValue, momentProvider.Now);

        await topicRepository.Create(topic, cancellationToken);
        await topicRepository.SaveChanges(cancellationToken);

        var result =  await topicRepository.GetAll()
            .AsNoTracking()
            .Where(t => t.Id == topicIdValue)
            .FirstAsync(cancellationToken);
        
        return new TopicCreateDto(result.Id, result.ForumId, result.UserId, result.Title.Value, result.Content.Value,
            result.CreatedAt);
    }
}