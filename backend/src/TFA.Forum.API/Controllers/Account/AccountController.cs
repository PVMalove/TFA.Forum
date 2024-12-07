using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TFA.Forum.API.Authentication;
using TFA.Forum.API.Controllers.Account.Request;
using TFA.Forum.API.Extensions;
using TFA.Forum.API.Response;
using TFA.Forum.Application.Commands.SingOut;

namespace TFA.Forum.API.Controllers.Account;

[ApiVersion("1.0")]
public class AccountController(IMediator mediator) : ApplicationController
{
    [HttpPost("sign_on")]
    public async Task<IActionResult> SignOn(
        [FromBody] SingOnRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await mediator.Send(command, cancellationToken);
        if (result.IsFailure) 
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpPost("sign_in")]
    public async Task<IActionResult> SignIn(
        [FromBody] SingInRequest request,
        [FromServices] IAuthTokenStorage tokenStorage,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await mediator.Send(command, cancellationToken);
        if (result.IsFailure) 
            return result.Error.ToResponse();
        
        tokenStorage.Store(HttpContext, result.Value.Token);

        return Ok(result.Value);
    }
    
    
    [HttpPost("sign_out")]
    public async Task<IActionResult> SignOut(
        [FromServices] SignOutUseCase useCase,
        [FromServices] IAuthTokenStorage tokenStorage,
        CancellationToken cancellationToken)
    {
        await useCase.Execute(new SignOutCommand(), cancellationToken);
        tokenStorage.Store(HttpContext, String.Empty);
        return Ok();
    }
}