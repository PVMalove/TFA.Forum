using CSharpFunctionalExtensions;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Persistence.Storage.Forum;

public interface IGetAllForumsStorage
{
    Task<IReadOnlyList<ForumGetDto>?> GetForums(CancellationToken cancellationToken);
    Task<Result<IReadOnlyList<ForumGetDto>, Error>> GetAllSortedForums(string? sortBy, string? sortDirection, CancellationToken cancellationToken);
}