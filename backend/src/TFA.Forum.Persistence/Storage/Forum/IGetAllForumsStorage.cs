using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Models;

namespace TFA.Forum.Persistence.Storage.Forum;

public interface IGetAllForumsStorage
{
    Task<IReadOnlyList<ForumGetDto>?> GetForums(CancellationToken cancellationToken);
    Task<PagedList<ForumsDto>> GetForumsWithPagination(string? sortBy, string? sortDirection, int page, int pageSize, CancellationToken token);
}