using Microsoft.AspNetCore.Mvc;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Queries.CreateTopic;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.Exceptions;

namespace TFA.Forum.API.Controllers.Forum.Request;


public class TopicController : ApplicationController
{
    [HttpPost("{forumId:guid}/topics")]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(CreateTopicRequest))]
    public async Task<IActionResult> CreateTopic(
        Guid forumId,
        [FromBody] CreateTopicRequest request,
        [FromServices] ICreateTopicUseCase useCase,
        CancellationToken cancellationToken)
    {
        try
        {
            var topic = await useCase.Execute(forumId, request.Title, request.Content, cancellationToken);
            return CreatedAtRoute(nameof(GetForums),new CreateTopicRequest
            {
                Title = topic.Title,
                Content = topic.Content,
            });
        }
        catch (Exception exception)
        {
            return exception switch
            {
                IntentionManagerException => Forbid(),
                ForumNotFoundException => StatusCode(StatusCodes.Status410Gone),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
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