namespace TFA.Forum.Domain.Interfaces.Repository;

public interface IBaseRepository<TEntity>
{
    IQueryable<TEntity> GetAll();
    Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken);
    TEntity Update(TEntity entity);
    void Remove(TEntity entity);
    Task<int> SaveChanges(CancellationToken cancellationToken);
}