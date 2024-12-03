using TFA.Forum.Application.Commands.SignIn;

namespace TFA.Forum.API.Controllers.Account.Request;

public record SingInRequest(string Login, string Password, bool RememberMe = false)
{
    public SignInCommand ToCommand() => 
        new(Login, Password, RememberMe);
}