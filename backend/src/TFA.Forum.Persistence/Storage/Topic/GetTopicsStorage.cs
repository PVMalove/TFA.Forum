using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Storage.Topic;

internal class GetTopicsStorage : IGetTopicsStorage
{
    private readonly IBaseRepository<Domain.Entities.Topic> topicRepository;

    public GetTopicsStorage(IBaseRepository<Domain.Entities.Topic> topicRepository)
    {
        this.topicRepository = topicRepository;
    }

    public async Task<(IEnumerable<Domain.Entities.Topic> resources, int totalCount)> GetTopics(
        Guid forumId, int skip, int take, CancellationToken cancellationToken)
    {
        var query = topicRepository.GetAll().Where(t => t.ForumId == forumId);
        var totalCount = await query.CountAsync(cancellationToken);

        var resources = await query
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(cancellationToken);

        return (resources, totalCount);
    }
}