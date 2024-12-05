using CSharpFunctionalExtensions;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Persistence.Storage.Forum;


public static class GetForumsStorageExtensions
{
    public static async Task<UnitResult<Error>> ThrowIfForumNotFound(this IGetAllForumsStorage storage, Guid forumId,
        CancellationToken cancellationToken)
    {
        if (!await ForumExists(storage, forumId, cancellationToken))
        {
            return Errors.General.NotFound(forumId);
        }
        
        return UnitResult.Success<Error>();
    }

    private static async Task<bool> ForumExists(this IGetAllForumsStorage storage, Guid forumId,
        CancellationToken cancellationToken)
    {
        var forums = await storage.GetForums(cancellationToken);
        return forums.Any(f => f.Id == forumId);
    }
}