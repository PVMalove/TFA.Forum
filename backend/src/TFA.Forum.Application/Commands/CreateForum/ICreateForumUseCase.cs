namespace TFA.Forum.Application.Commands.CreateForum;

public interface ICreateForumUseCase
{
    Task<Domain.Entities.Forum> Execute(CreateForumCommand command, CancellationToken cancellationToken);
}