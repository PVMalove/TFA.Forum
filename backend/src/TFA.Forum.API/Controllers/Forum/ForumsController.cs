using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TFA.Forum.API.Controllers.Forum.Request;
using TFA.Forum.API.Extensions;
using TFA.Forum.Application.Commands.CreateForum;
using TFA.Forum.Application.Commands.CreateTopic;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Application.Queries.GetTopics;

namespace TFA.Forum.API.Controllers.Forum;

[ApiVersion("1.0")]
public class ForumsController : ApplicationController
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(GetSortedForumsRequest[]))]
    public async Task<IActionResult> GetForums(
        [FromQuery] GetSortedForumsRequest request,
        [FromServices] GetAllForumsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request.ToQuery(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost]
    [ProducesResponseType(400)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(CreateTopicRequest))]
    public async Task<ActionResult> CreateForums(
        [FromBody] CreateForumRequest request,
        [FromServices] CreateForumUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request.ToCommand(), cancellationToken);
        if (result.IsFailure) 
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet("{forumId:guid}/topics")]
    [ProducesResponseType(400)]
    [ProducesResponseType(410)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetTopics(
        [FromRoute] Guid forumId,
        [FromQuery] GetTopicsWithPaginationRequest request,
        [FromServices] GetTopicsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request.ToQuery(forumId), cancellationToken);
        if (result.IsFailure) 
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{forumId:guid}/topics")]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(CreateTopicRequest))]
    public async Task<IActionResult> CreateTopics(
        [FromRoute] Guid forumId,
        [FromBody] CreateTopicRequest request,
        [FromServices] CreateTopicUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request.ToCommand(forumId), cancellationToken);
        if (result.IsFailure) 
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}