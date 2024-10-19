using TFA.Forum.Domain.Entities;

namespace TFA.Forum.Application.Queries.CreateTopic;

public interface ICreateTopicUseCase
{
    Task<Topic> Execute(CreateTopicQuery query, CancellationToken cancellationToken);
}