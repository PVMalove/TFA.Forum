using TFA.Forum.Application.Commands.SignOn;

namespace TFA.Forum.API.Controllers.Account.Request;

public record SingOnRequest(string Login, string Password)
{
    public SignOnCommand ToCommand() => 
        new(Login, Password);
}