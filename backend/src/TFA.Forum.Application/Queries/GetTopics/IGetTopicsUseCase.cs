using TFA.Forum.Domain.Entities;

namespace TFA.Forum.Application.Queries.GetTopics;


public interface IGetTopicsUseCase
{
    Task<(IEnumerable<Topic> resources, int totalCount)> Execute(GetTopicsQuery query, 
        CancellationToken cancellationToken);
}