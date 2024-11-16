namespace TFA.Forum.Persistence.Storage.Forum;

public interface IGetAllForumsStorage
{
    Task<IEnumerable<Domain.Entities.Forum>?> GetForums(CancellationToken cancellationToken);
}