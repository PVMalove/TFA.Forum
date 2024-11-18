using TFA.Forum.Domain.DTO.Forum;

namespace TFA.Forum.Persistence.Storage.Forum;

public interface ICreateForumStorage
{
    Task<ForumCreateDto> Create(string? title, CancellationToken cancellationToken);
}