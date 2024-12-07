using Microsoft.AspNetCore.Mvc;
using TFA.Forum.API.Response;

namespace TFA.Forum.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);
        
        return new OkObjectResult(envelope);
    }
}