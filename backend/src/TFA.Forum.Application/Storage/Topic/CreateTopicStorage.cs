using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain;
using TFA.Forum.Domain.Interfaces;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Application.Storage.Topic;

public class CreateTopicStorage: ICreateTopicStorage
{
    private readonly IBaseRepository<Domain.Entities.Forum> forumRepository;
    private readonly IBaseRepository<Domain.Entities.Topic> topicRepository;
    private readonly IGuidFactory guidFactory;
    private readonly IMomentProvider momentProvider;

    public CreateTopicStorage(IBaseRepository<Domain.Entities.Forum> forumRepository, IBaseRepository<Domain.Entities.Topic> topicRepository, IGuidFactory guidFactory, IMomentProvider momentProvider)
    {
        this.forumRepository = forumRepository;
        this.topicRepository = topicRepository;
        this.guidFactory = guidFactory;
        this.momentProvider = momentProvider;
    }

    public Task<bool> ForumExists(Guid forumId, CancellationToken cancellationToken) =>
        forumRepository.GetAll().AnyAsync(f => f.Id == forumId, cancellationToken);

    public async Task<Domain.Entities.Topic> CreateTopic(Guid forumId, Guid AuthorId, string? title,
        string? content, CancellationToken cancellationToken)
    {
        var topicId = guidFactory.Create();
        var topic = new Domain.Entities.Topic
        {
            Id = topicId,
            ForumId = forumId,
            AuthorId = AuthorId,
            Title = title,
            Content = content,
            CreateAt = momentProvider.Now,
        };

        await topicRepository.CreateAsync(topic, cancellationToken);
        await topicRepository.SaveChangesAsync(cancellationToken);

        return await topicRepository.GetAll()
            .AsNoTracking()
            .Where(t => t.Id == topicId)
            .FirstAsync(cancellationToken);
    }
}