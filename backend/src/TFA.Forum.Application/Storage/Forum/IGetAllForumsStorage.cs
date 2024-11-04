namespace TFA.Forum.Application.Storage.Forum;

public interface IGetAllForumsStorage
{
    Task<IEnumerable<Domain.Entities.Forum>?> GetForums(CancellationToken cancellationToken);
}