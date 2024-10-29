namespace TFA.Forum.Application.Storage.Topic;


public interface IGetTopicsStorage
{
    Task<(IEnumerable<Domain.Entities.Topic> resources, int totalCount)> GetTopics(
        Guid forumId, int skip, int take, CancellationToken cancellationToken);
}