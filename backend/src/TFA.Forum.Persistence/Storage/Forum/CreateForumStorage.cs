using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Forum.Domain.Interfaces;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Storage.Forum;

public class CreateForumStorage : ICreateForumStorage
{
    private readonly IBaseRepository<Domain.Entities.Forum> forumRepository;
    private readonly IGuidFactory guidFactory;
    private readonly IMomentProvider momentProvider;
    private readonly IMemoryCache memoryCache;

    public CreateForumStorage(IBaseRepository<Domain.Entities.Forum> forumRepository, IGuidFactory guidFactory, 
        IMomentProvider momentProvider, IMemoryCache memoryCache)
    {
        this.forumRepository = forumRepository;
        this.guidFactory = guidFactory;
        this.momentProvider = momentProvider;
        this.memoryCache = memoryCache;
    }

    public async Task<Domain.Entities.Forum> Create(string? title, CancellationToken cancellationToken)
    {
        var forumId = guidFactory.Create();
        var forum = new Domain.Entities.Forum
        {
            Id = forumId,
            Title = title
        };
        
        await forumRepository.CreateAsync(forum, cancellationToken);
        await forumRepository.SaveChangesAsync(cancellationToken);
        
        memoryCache.Remove(nameof(GetAllForumsStorage.GetForums));

        return await forumRepository.GetAll()
            .AsNoTracking()
            .Where(t => t.Id == forumId)
            .FirstAsync(cancellationToken);
    }
}