using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.DTO.Topic;
using TFA.Forum.Domain.Interfaces.Repository;
using TFA.Forum.Domain.Models;
using TFA.Forum.Persistence.Extensions;

namespace TFA.Forum.Persistence.Storage.Topic;

internal class GetTopicsStorage : IGetTopicsStorage
{
    private readonly IBaseRepository<Domain.Entities.Topic> topicRepository;

    public GetTopicsStorage(IBaseRepository<Domain.Entities.Topic> topicRepository)
    {
        this.topicRepository = topicRepository;
    }

    public Task<PagedList<TopicGetDto>> GetTopicsWithPagination(Guid forumId, string? sortBy, string? sortDirection, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var query = topicRepository.GetAll()
            .AsNoTracking()
            .Where(t => t.ForumId == forumId);
        
        var keySelector = SortByProperty(sortBy);
        
        query = sortDirection?.ToLower() == "desc"
            ? query .OrderByDescending(keySelector)
            : query .OrderBy(keySelector);
        
        var topicsQuery = query.Select(t => new TopicGetDto(t.Id, t.ForumId, t.UserId, t.Title.Value, t.Content.Value, t.CreatedAt));
        
        var result = topicsQuery.GetObjectsWithPagination(page, pageSize, cancellationToken);
        return result;
    }
    
    private static Expression<Func<Domain.Entities.Topic, object>> SortByProperty(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return topic => topic.Id;

        Expression<Func<Domain.Entities.Topic, object>> keySelector = sortBy?.ToLower() switch
        {
            "title" => topic => topic.Title.Value,
            "created" => topic => topic.CreatedAt,
            _ => topic => topic.Id
        };
        return keySelector;
    }
}