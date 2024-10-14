using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Application.Storage.Forum;

public class GetAllForumsStorage : IGetAllForumsStorage
{
    private readonly IBaseRepository<Domain.Entities.Forum> forumRepository;

    public GetAllForumsStorage(IBaseRepository<Domain.Entities.Forum> forumRepository)
    {
        this.forumRepository = forumRepository;
    }

    public async Task<IEnumerable<Domain.Entities.Forum>> GetForums(CancellationToken cancellationToken)
    {
        return await forumRepository.GetAll()
            .Select(f => new Domain.Entities.Forum
            {
                Id = f.Id,
                Title = f.Title
            })
            .ToArrayAsync(cancellationToken);
    }
}