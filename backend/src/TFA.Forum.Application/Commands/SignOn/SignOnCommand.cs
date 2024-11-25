using TFA.Forum.Application.Abstractions;

namespace TFA.Forum.Application.Commands.SignOn;

public record SignOnCommand(string Login, string Password) : ICommand;