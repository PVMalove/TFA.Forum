using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Storage.Forum;

public class GetAllForumsStorage : IGetAllForumsStorage
{
    private readonly IBaseRepository<Domain.Entities.Forum> forumRepository;
    private readonly IMemoryCache memoryCache;

    public GetAllForumsStorage(IBaseRepository<Domain.Entities.Forum> forumRepository,
        IMemoryCache memoryCache)
    {
        this.forumRepository = forumRepository;
        this.memoryCache = memoryCache;
    }

    public async Task<IReadOnlyList<ForumGetDto>?> GetForums(CancellationToken cancellationToken)
    {
        return await memoryCache.GetOrCreateAsync<ForumGetDto[]>(
            nameof(GetForums),
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                var result = await forumRepository.GetAll()
                    .AsNoTracking()
                    .ToArrayAsync(cancellationToken);

                return result.Select(forum =>
                    new ForumGetDto(forum.Id, forum.Title.Value, forum.CreatedAt)).ToArray();
            });
    }

    public async Task<IReadOnlyList<ForumGetDto>> GetAllSortedForums(string? sortBy, string? sortDirection,
        CancellationToken cancellationToken)
    {
        var query = forumRepository.GetAll().AsNoTracking();
        
        var keySelector = SortByProperty(sortBy);
        
        query = sortDirection?.ToLower() == "desc"
            ? query .OrderByDescending(keySelector)
            : query .OrderBy(keySelector);

        var result = await query.Select(f => new ForumGetDto(f.Id, f.Title.Value, f.CreatedAt)).ToArrayAsync(cancellationToken);

        return result;
    }
    
    private static Expression<Func<Domain.Entities.Forum, object>> SortByProperty(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return forum => forum.Id;

        Expression<Func<Domain.Entities.Forum, object>> keySelector = sortBy?.ToLower() switch
        {
            "title" => forum => forum.Title.Value,
            "created" => forum => forum.CreatedAt,
            _ => forum => forum.Id
        };
        return keySelector;
    }
}

