using DrMW.Repositories.Abstractions.Components.Common.Writes;
using DrMW.Repositories.Concretes.Components.Common.Reads;
using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Concretes.Components.Common.Writes;

public class WriteAnonymousRepository<TEntity> : BaseRepository<TEntity>,IWriteAnonymousRepository<TEntity> 
    where TEntity : class
{
   
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOriginRepository{TEntity,TPrimary}"/> class.
    /// </summary>
    /// <param name="database">The database context to be used by the repository.</param>
    public WriteAnonymousRepository(IWriteDatabase database) : base(database.Context)
    {
    }

    protected internal WriteAnonymousRepository(DbContext context) : base(context)
    {
    }


    /// <summary>
    /// Asynchronously adds a new entity to the DbSet.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added entity.</returns>
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await Table.AddAsync(entity);
        return entity;
    }


    /// <summary>
    /// Asynchronously adds a range of entities to the DbSet.
    /// </summary>
    /// <param name="entities">The list of entities to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of added entities.</returns>
    public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
    {
        await Table.AddRangeAsync(entities);
        return entities;
    }


    /// <summary>
    /// Asynchronously updates an existing entity in the DbSet.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity.</returns>
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        Table.Update(entity);
        return entity;
    }


    /// <summary>
    /// Asynchronously updates a range of entities in the DbSet.
    /// </summary>
    /// <param name="entities">The list of entities to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of updated entities.</returns>
    public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities)
    {
        Table.UpdateRange(entities);
        return entities;
    }

    /// <summary>
    /// Hard Delete | Asynchronously removes an entity from the DbSet. This is a hard delete operation.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the removed entity.</returns>
    public async Task<TEntity> RemoveAsync(TEntity entity) // Hard Delete
    {
        Table.Remove(entity);
        return entity;
    }


    /// <summary>
    /// Hard Delete | Asynchronously removes a range of entities from the DbSet. This is a hard delete operation.
    /// </summary>
    /// <param name="entities">The list of entities to remove.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of removed entities.</returns>
    public async Task<List<TEntity>> RemoveRangeAsync(List<TEntity> entities) // Hard Delete
    {
        Table.RemoveRange(entities);
        return entities;
    }

    
    /// <summary>
    /// Destructor for ReadRepository.
    /// </summary>
    ~WriteAnonymousRepository()
    {
        Dispose(false);
    }

}