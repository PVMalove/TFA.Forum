using CSharpFunctionalExtensions;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Monitoring;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.Forum;

namespace TFA.Forum.Application.Queries.GetAllForums;

public class GetAllForumsUseCase(IGetAllForumsStorage storage, DomainMetrics metrics) : IQueryHandler<IReadOnlyList<ForumGetDto>, GetAllSortedForumsQuery>
{
    public async Task<Result<IReadOnlyList<ForumGetDto>, ErrorList>> Execute(GetAllSortedForumsQuery query, CancellationToken cancellationToken)
    {
        var result = await storage.GetAllSortedForums(query.SortBy, query.SortDirection, cancellationToken);
        metrics.ForumsFetched(true);
        return result.ToList().AsReadOnly();
    }
}