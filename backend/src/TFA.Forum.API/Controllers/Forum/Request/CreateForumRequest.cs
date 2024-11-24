using TFA.Forum.Application.Commands.CreateForum;

namespace TFA.Forum.API.Controllers.Forum.Request;

public record CreateForumRequest(string? Title)
{
    public CreateForumCommand ToCommand() =>
        new(Title);
}