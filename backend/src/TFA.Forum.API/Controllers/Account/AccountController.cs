using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TFA.Forum.API.Authentication;
using TFA.Forum.API.Controllers.Account.Request;
using TFA.Forum.API.Extensions;
using TFA.Forum.Application.Commands.SignIn;

namespace TFA.Forum.API.Controllers.Account;

[ApiVersion("1.0")]
public class AccountController : ApplicationController
{
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(
        [FromBody] SingInRequest request,
        [FromServices] SignInUseCase useCase,
        [FromServices] IAuthTokenStorage tokenStorage,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request.ToCommand(), cancellationToken);
        
        tokenStorage.Store(HttpContext, result.Value.Token);
        
        if (result.IsFailure) 
            return result.Error.ToResponse();
        
        return Ok(result.Value.Identity);
    }
}