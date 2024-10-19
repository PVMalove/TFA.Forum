namespace TFA.Forum.Application.Queries.CreateTopic;

public record CreateTopicQuery(Guid ForumId, string? Title, string? Content);
