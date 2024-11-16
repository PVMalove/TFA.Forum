using TFA.Forum.Domain.Exceptions;
using TFA.Forum.Persistence.Storage.Forum;

namespace TFA.Forum.Application.Queries.GetAllForums;


internal static class GetForumsStorageExtensions
{
    public static async Task ThrowIfForumNotFound(this IGetAllForumsStorage storage, Guid forumId,
        CancellationToken cancellationToken)
    {
        if (!await ForumExists(storage, forumId, cancellationToken))
        {
            throw new ForumNotFoundException(forumId);
        }
    }

    private static async Task<bool> ForumExists(this IGetAllForumsStorage storage, Guid forumId,
        CancellationToken cancellationToken)
    {
        var forums = await storage.GetForums(cancellationToken);
        return forums.Any(f => f.Id == forumId);
    }
}