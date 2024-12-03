using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TFA.Forum.API.Authentication;
using TFA.Forum.API.Controllers.Account.Request;
using TFA.Forum.API.Extensions;
using TFA.Forum.API.Response;
using TFA.Forum.Application.Commands.SignIn;
using TFA.Forum.Application.Commands.SignOn;
using TFA.Forum.Application.Commands.SingOut;

namespace TFA.Forum.API.Controllers.Account;

[ApiVersion("1.0")]
public class AccountController : ApplicationController
{
    [HttpPost("sign_on")]
    public async Task<IActionResult> SignOn(
        [FromBody] SingOnRequest request,
        [FromServices] SignOnUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request.ToCommand(), cancellationToken);
        if (result.IsFailure) 
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPost("sign_in")]
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
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    
    [HttpPost("sign_out")]
    public async Task<IActionResult> SignOut(
        [FromServices] SignOutUseCase useCase,
        CancellationToken cancellationToken)
    {
        await useCase.Execute(new SignOutCommand(), cancellationToken);
        return Ok(Envelope.Ok());
    }
}