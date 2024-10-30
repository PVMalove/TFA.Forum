using TFA.Forum.Application.Queries.GetTopics;

namespace TFA.Forum.API.Controllers.Forum.Request;

public record GetTopicRequest(int Skip, int Take)
{
    public GetTopicsQuery ToQuery(Guid ForumId) 
        => new(ForumId, Skip, Take);
}