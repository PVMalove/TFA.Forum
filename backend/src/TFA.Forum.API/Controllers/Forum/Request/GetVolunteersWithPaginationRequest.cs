using TFA.Forum.Application.Queries.GetAllForums;

namespace TFA.Forum.API.Controllers.Forum.Request;

public record GetForumsWithPaginationRequest(
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetAllForumsWithPaginationQuery ToQuery() =>
        new(
            SortBy,
            SortDirection,
            Page,
            PageSize);
}