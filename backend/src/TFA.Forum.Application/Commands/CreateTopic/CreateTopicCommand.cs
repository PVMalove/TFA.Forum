using TFA.Forum.Application.Abstractions;

namespace TFA.Forum.Application.Commands.CreateTopic;

public record CreateTopicCommand(Guid ForumId, string? Title, string? Content) : ICommand;
