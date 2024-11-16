using TFA.Forum.Domain.DTO.Forum;

namespace TFA.Forum.Application.Commands.CreateForum;

public interface ICreateForumUseCase
{
    Task<ForumCreateDto> Execute(CreateForumCommand command, CancellationToken cancellationToken);
}