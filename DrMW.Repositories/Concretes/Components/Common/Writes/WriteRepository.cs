using System.Linq.Expressions;
using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Abstractions.Components.Common.Writes;
using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Concretes.Components.Common.Writes;

/// <summary>
/// Represents a generic repository for write operations on entities of type TEntity with primary key of type TPrimary.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TPrimary">The type of the primary key for the entity.</typeparam>
public class WriteRepository<TEntity, TPrimary> : WriteOriginRepository<TEntity,TPrimary>,IWriteRepository<TEntity, TPrimary>
    where TEntity : class, IBaseEntity<TPrimary>, new()
{

    /// <summary>
    /// Initializes a new instance of the <see cref="WriteRepository{TEntity,TPrimary}"/> class.
    /// </summary>
    /// <param name="database">The database context to be used by the repository.</param>
    public WriteRepository(IWriteDatabase database) : base(database)
    {
    }
    
    protected internal WriteRepository(DbContext dbContext) : base(dbContext)
    {
    }
    
    /// <summary>
    /// Asynchronously marks an entity as deleted in the DbSet. This is a soft delete operation.
    /// </summary>
    /// <param name="entity">The entity to mark as deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deleted entity.</returns>
    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        entity.IsDeleted = true;
        Table.Update(entity);
        return entity;
    }


    /// <summary>
    /// Asynchronously marks a range of entities as deleted in the DbSet. This is a soft delete operation.
    /// </summary>
    /// <param name="entities">The list of entities to mark as deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of deleted entities.</returns>
    public async Task<List<TEntity>> DeleteRangeAsync(List<TEntity> entities)
    {
        entities.ForEach(e => e.IsDeleted = true);
        Table.UpdateRange(entities);
        return entities;
    }


    /// <summary>
    /// Asynchronously marks entities as deleted based on a predicate. This is a soft delete operation.
    /// </summary>
    /// <param name="predicate">An expression to filter the entities to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        List<TEntity> entities = Table.Where(predicate).ToList();
        await DeleteRangeAsync(entities);
    }


    /// <summary>
    /// Asynchronously marks an entity as deleted based on its identifier. This is a soft delete operation.
    /// </summary>
    /// <param name="id">The identifier of the entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deleted entity, or null if the entity is not found.</returns>
    public async Task<TEntity> DeleteByIdAsync(TPrimary id)
    {
        var entity = await Table.FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (entity == null) return null;
        return await DeleteAsync(entity);
    }

    

    /// <summary>
    /// Destructor for WriteRepository.
    /// </summary>
    ~WriteRepository()
    {
        Dispose(false);
    }

}