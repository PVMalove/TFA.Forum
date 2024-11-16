using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Storage.Forum;

public class GetAllForumsStorage : IGetAllForumsStorage
{
    private readonly IBaseRepository<Domain.Entities.Forum> forumRepository;
    private readonly IMemoryCache memoryCache;

    public GetAllForumsStorage(IBaseRepository<Domain.Entities.Forum> forumRepository, IMemoryCache memoryCache)
    {
        this.forumRepository = forumRepository;
        this.memoryCache = memoryCache;
    }

    public async Task<IEnumerable<Domain.Entities.Forum>?> GetForums(CancellationToken cancellationToken)
    {
        return await memoryCache.GetOrCreateAsync<Domain.Entities.Forum[]>(
            nameof(GetForums),
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return forumRepository.GetAll().ToArrayAsync(cancellationToken);
            });
    }
}