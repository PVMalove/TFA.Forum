using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.EntityIds;
using TFA.Forum.Domain.Interfaces;
using TFA.Forum.Domain.Interfaces.Repository;
using TFA.Forum.Domain.ValueObjects;

namespace TFA.Forum.Persistence.Storage.Forum;

public class CreateForumStorage(
    IBaseRepository<Domain.Entities.Forum> forumRepository,
    IMomentProvider momentProvider,
    IMemoryCache memoryCache)
    : ICreateForumStorage
{
    public async Task<ForumCreateDto> Create(string? title, CancellationToken cancellationToken)
    {
        var forumId = ForumId.NewId();
        var forumTitle = Title.Create(title).Value;

        var forum = Domain.Entities.Forum.Create(forumId, forumTitle, momentProvider.Now);

        await forumRepository.Create(forum, cancellationToken);
        await forumRepository.SaveChanges(cancellationToken);
        
        memoryCache.Remove(nameof(GetAllForumsStorage.GetForums));

        var result = await forumRepository.GetAll()
            .AsNoTracking()
            .Where(t => t.Id == forumId)
            .FirstAsync(cancellationToken);
        
        return new ForumCreateDto(result.Id, result.Title.Value, result.CreatedAt);
    }
}