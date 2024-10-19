using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TFA.Forum.Application.Queries.CreateTopic;
using TFA.Forum.Application.Queries.GetAllForums;

namespace TFA.Forum.API.Controllers.Forum.Request;

[ApiVersion("1.0")]
public class ForumController : ApplicationController
{
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
        var query = new CreateTopicQuery(forumId, request.Title, request.Content);
        var topic = await useCase.Execute(query, cancellationToken);
        return CreatedAtRoute(nameof(GetForums),new CreateTopicRequest
        {
            Title = topic.Title,
            Content = topic.Content,
        });
    }
    
    [HttpGet("GetForums")]
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
}