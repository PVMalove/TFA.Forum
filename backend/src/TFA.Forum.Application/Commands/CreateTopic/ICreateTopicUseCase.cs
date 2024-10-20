using TFA.Forum.Domain.Entities;

namespace TFA.Forum.Application.Commands.CreateTopic;

public interface ICreateTopicUseCase
{
    Task<Topic> Execute(CreateTopicCommand command, CancellationToken cancellationToken);
}