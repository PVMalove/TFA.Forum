using TFA.Forum.Application.Abstractions;

namespace TFA.Forum.Application.Commands.SignIn;

public record SignInCommand(string Login, string Password, bool RememberMe) : ICommand;