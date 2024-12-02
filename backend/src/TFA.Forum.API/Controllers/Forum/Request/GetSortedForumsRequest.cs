using TFA.Forum.Application.Queries.GetAllForums;

namespace TFA.Forum.API.Controllers.Forum.Request;

public record GetSortedForumsRequest(
    string? SortBy,
    string? SortDirection)
{
    public GetAllSortedForumsQuery ToQuery() =>
        new(
            SortBy,
            SortDirection);
}