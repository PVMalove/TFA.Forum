using TFA.Forum.Application.Abstractions;

namespace TFA.Forum.Application.Queries.GetAllForums;

public record GetAllSortedForumsQuery(
    string? SortBy,
    string? SortDirection): IQuery;