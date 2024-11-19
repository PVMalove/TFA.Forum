using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TFA.Forum.API.Controllers.Forum.Request;
using TFA.Forum.API.Extensions;
using TFA.Forum.API.Response;
using TFA.Forum.Application.Commands.CreateForum;
using TFA.Forum.Application.Commands.CreateTopic;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Application.Queries.GetTopics;


namespace TFA.Forum.API.Controllers.Forum;

[ApiVersion("1.0")]
public class ForumController : ApplicationController
{
    [HttpGet(Name = "all_forums")]
    [ProducesResponseType(200, Type = typeof(GetAllForumRequest[]))]
    public async Task<IActionResult> GetForums(
        [FromServices] IGetAllForumsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var forums = await useCase.Execute(cancellationToken);
        return Ok(forums?.Select(f => new GetAllForumRequest(f.Id, f.Title.Value)));
    }
    
    [HttpPost("create")]
    [ProducesResponseType(400)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(CreateTopicRequest))]
    public async Task<ActionResult> CreateForum(
        [FromBody] CreateForumRequest request,
        [FromServices] CreateForumUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request.ToCommand(), cancellationToken);
        
        if (result.IsFailure) 
            return result.Error.ToResponse();

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpGet("{forumId:guid}/topics")]
    [ProducesResponseType(400)]
    [ProducesResponseType(410)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetTopics(
        [FromRoute] Guid forumId,
        [FromQuery] GetTopicRequest request,
        [FromServices] IGetTopicsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var (resources, totalCount) = await useCase.Execute(request.ToQuery(forumId), cancellationToken);
        return Ok(new { resources = resources.Select(t => new CreateTopicRequest(t.Title, t.Content)), totalCount });
    }

    [HttpPost("{forumId:guid}/topic")]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(CreateTopicRequest))]
    public async Task<IActionResult> CreateTopic(
        [FromRoute] Guid forumId,
        [FromBody] CreateTopicRequest request,
        [FromServices] ICreateTopicUseCase useCase,
        CancellationToken cancellationToken)
    {
        var topic = await useCase.Execute(request.ToCommand(forumId), cancellationToken);
        return CreatedAtRoute("all_forums", new CreateTopicRequest(topic.Title, topic.Content));
    }
}