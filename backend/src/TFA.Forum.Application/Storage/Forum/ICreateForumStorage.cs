namespace TFA.Forum.Application.Storage.Forum;

public interface ICreateForumStorage
{
    Task<Domain.Entities.Forum> Create(string? title, CancellationToken cancellationToken);
}