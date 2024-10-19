namespace TFA.Forum.API.Controllers.Forum.Request;

public class CreateTopicRequest
{
    public string? Title { get; init; }
    public string? Content { get; init; }
}