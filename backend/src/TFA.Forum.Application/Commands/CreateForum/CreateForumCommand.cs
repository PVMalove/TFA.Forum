using TFA.Forum.Application.Abstractions;

namespace TFA.Forum.Application.Commands.CreateForum;

public record CreateForumCommand(string? Title)  : ICommand;