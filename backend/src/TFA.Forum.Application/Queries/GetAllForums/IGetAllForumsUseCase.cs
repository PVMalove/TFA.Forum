namespace TFA.Forum.Application.Queries.GetAllForums;

public interface IGetAllForumsUseCase
{
    Task<IEnumerable<Domain.Entities.Forum>> Execute(CancellationToken cancellationToken);
}