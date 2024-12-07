using TFA.Forum.Domain.Interfaces.Repository;

namespace TFA.Forum.Persistence.Repositories;

public class BaseRepository<TEntity>(ApplicationDbContext dbContext) : IBaseRepository<TEntity>
    where TEntity : class
{
    public IQueryable<TEntity> GetAll()
    {
        return dbContext.Set<TEntity>();
    }

    public async Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await dbContext.AddAsync(entity, cancellationToken);
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        dbContext.Update(entity);
        return entity;
    }

    public void Remove(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        dbContext.Remove(entity);
    }
    
    public async Task<int> SaveChanges(CancellationToken cancellationToken)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}