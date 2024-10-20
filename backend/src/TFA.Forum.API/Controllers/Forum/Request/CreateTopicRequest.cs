using TFA.Forum.Application.Commands.CreateTopic;

namespace TFA.Forum.API.Controllers.Forum.Request;

public record CreateTopicRequest(string? Title, string? Content)
{
    public CreateTopicCommand ToCommand(Guid ForumId)
        => new(ForumId, Title, Content);
}