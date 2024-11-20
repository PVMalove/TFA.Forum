using TFA.Forum.Domain.Shared;

namespace TFA.Forum.E2E;

public record ApiResponse<T>
{
    public T? Result { get; init; }
    public ErrorList? Errors { get; init; }
    public DateTimeOffset TimeCreated { get; init; }
}