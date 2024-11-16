using TFA.Forum.Persistence.Storage.Forum;

namespace TFA.Forum.Application.Queries.GetAllForums;

public class GetAllForumsUseCase(IGetAllForumsStorage storage) : IGetAllForumsUseCase
{
    public Task<IEnumerable<Domain.Entities.Forum>?> Execute(CancellationToken cancellationToken) =>
        storage.GetForums(cancellationToken);
}