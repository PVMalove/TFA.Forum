using TFA.Forum.Application.Queries.GetTopics;

namespace TFA.Forum.API.Controllers.Forum.Request;

public record GetTopicsWithPaginationRequest(string? SortBy,
    string? SortDirection, int Page, int PageSize)
{
    public GetTopicsWithPaginationQuery ToQuery(Guid ForumId) =>
        new(
            ForumId,
            SortBy,
            SortDirection,
            Page,
            PageSize);
}