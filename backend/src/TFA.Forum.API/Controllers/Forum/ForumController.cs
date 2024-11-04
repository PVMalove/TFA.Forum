using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TFA.Forum.API.Controllers.Forum.Request;
using TFA.Forum.Application.Commands.CreateTopic;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Application.Queries.GetTopics;
using TFA.Forum.Domain.Entities;

namespace TFA.Forum.API.Controllers.Forum;

[ApiVersion("1.0")]
public class ForumController : ApplicationController
{
    [HttpGet(Name = nameof(GetForums))]
    [ProducesResponseType(200, Type = typeof(GetAllForumRequest[]))]
    public async Task<IActionResult> GetForums(
        [FromServices] IGetAllForumsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var forums = await useCase.Execute(cancellationToken);
        return Ok(forums.Select(f => new GetAllForumRequest
        {
            Id = f.Id,
            Title = f.Title
        }));
    }
    
    [HttpPost("{forumId:guid}/topics")]
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
        return CreatedAtRoute(nameof(GetForums), new CreateTopicRequest(topic.Title, topic.Content));
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
}