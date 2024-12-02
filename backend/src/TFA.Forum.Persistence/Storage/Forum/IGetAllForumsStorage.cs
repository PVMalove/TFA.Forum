using TFA.Forum.Domain.DTO.Forum;

namespace TFA.Forum.Persistence.Storage.Forum;

public interface IGetAllForumsStorage
{
    Task<IReadOnlyList<ForumGetDto>?> GetForums(CancellationToken cancellationToken);
    Task<IReadOnlyList<ForumGetDto>> GetAllSortedForums(string? sortBy, string? sortDirection, CancellationToken cancellationToken);
}