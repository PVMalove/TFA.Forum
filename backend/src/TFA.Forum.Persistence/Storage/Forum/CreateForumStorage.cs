using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Interfaces;
using TFA.Forum.Domain.Interfaces.Repository;
using TFA.Forum.Domain.ValueObjects;

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

    public async Task<ForumCreateDto> Create(string? title, CancellationToken cancellationToken)
    {
        var forumId = guidFactory.Create();
        var forumTitle = Title.Create(title).Value;
        
        var forum = new Domain.Entities.Forum(forumId, forumTitle, momentProvider.Now);

        await forumRepository.Create(forum, cancellationToken);
        await forumRepository.SaveChanges(cancellationToken);
        
        memoryCache.Remove(nameof(GetAllForumsStorage.GetForums));

        var result = await forumRepository.GetAll()
            .AsNoTracking()
            .Where(t => t.Id == forumId)
            .FirstAsync(cancellationToken);
        
        return new ForumCreateDto(result.Title.Value, result.CreatedAt);
    }
}