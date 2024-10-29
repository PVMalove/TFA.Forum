namespace TFA.Forum.Application.Queries.GetTopics;

public record GetTopicsQuery(Guid ForumId, int Skip, int Take);