using CSharpFunctionalExtensions;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.Forum;

namespace TFA.Forum.Application.Queries.GetAllForums;

public class GetAllForumsUseCase(IGetAllForumsStorage storage) : IQueryHandler<IReadOnlyList<ForumGetDto>, GetAllSortedForumsQuery>
{
    public async Task<Result<IReadOnlyList<ForumGetDto>, ErrorList>> Execute(GetAllSortedForumsQuery query, CancellationToken token)
    {
        var result = await storage.GetAllSortedForums(query.SortBy, query.SortDirection, token);
        return result.ToList();
    }
}