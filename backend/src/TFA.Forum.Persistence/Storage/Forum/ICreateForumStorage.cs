namespace TFA.Forum.Persistence.Storage.Forum;

public interface ICreateForumStorage
{
    Task<Domain.Entities.Forum> Create(string? title, CancellationToken cancellationToken);
}