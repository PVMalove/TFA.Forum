using TFA.Forum.Application.Abstractions;

namespace TFA.Forum.Application.Queries.GetAllForums;

public record GetAllForumsWithPaginationQuery(
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize): IQuery;