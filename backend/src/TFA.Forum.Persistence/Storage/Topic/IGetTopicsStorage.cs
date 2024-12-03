using TFA.Forum.Domain.DTO.Topic;
using TFA.Forum.Domain.Models;

namespace TFA.Forum.Persistence.Storage.Topic;


public interface IGetTopicsStorage
{
    Task<PagedList<TopicGetDto>> GetTopicsWithPagination(Guid forumId, string? sortBy, string? sortDirection, int page, int pageSize, CancellationToken cancellationToken);
}