using TFA.Forum.Application.Abstractions;

namespace TFA.Forum.Application.Queries.GetTopics;

public record GetTopicsWithPaginationQuery(
    Guid ForumId,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;