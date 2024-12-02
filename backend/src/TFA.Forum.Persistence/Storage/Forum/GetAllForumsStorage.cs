using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Interfaces.Repository;
using TFA.Forum.Domain.Models;
using TFA.Forum.Persistence.Extensions;

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

    public async Task<PagedList<ForumsDto>> GetForumsWithPagination(string? sortBy, string? sortDirection, int page, int pageSize,
        CancellationToken token)
    {
        var query  = forumRepository.GetAll().AsNoTracking();
        
        var keySelector = SortByProperty(sortBy);
        
        query = sortDirection?.ToLower() == "desc"
            ? query .OrderByDescending(keySelector)
            : query .OrderBy(keySelector);

        var forumsQuery = query.Select(f => new ForumsDto(f.Id, f.Title.Value, f.CreatedAt));

        var result = await forumsQuery.GetObjectsWithPagination(page, pageSize, token);
        return result;
    }
    
    private static Expression<Func<Domain.Entities.Forum, object>> SortByProperty(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return volunteer => volunteer.Id;

        Expression<Func<Domain.Entities.Forum, object>> keySelector = sortBy?.ToLower() switch
        {
            "title" => forum => forum.Title.Value,
            "CreatedAt" => forum => forum.CreatedAt,
            _ => forum => forum.Id
        };
        return keySelector;
    }
}

