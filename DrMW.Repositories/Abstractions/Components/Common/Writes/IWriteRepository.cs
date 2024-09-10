using System.Linq.Expressions;
using DrMW.Core.Models.Abstractions;

namespace DrMW.Repositories.Abstractions.Components.Common.Writes;
/// <summary>
/// Represents a generic repository for write operations on entities of type TEntity with primary key of type TPrimary.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TPrimary">The type of the primary key for the entity.</typeparam>
public interface IWriteRepository<TEntity, in TPrimary> : IWriteOriginRepository<TEntity, TPrimary>
    where TEntity : class, IBaseEntity<TPrimary>

{
    
    /// <summary>
    /// Asynchronously marks an entity as deleted in the DbSet. This is a soft delete operation.
    /// </summary>
    /// <param name="entity">The entity to mark as deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deleted entity.</returns>
    Task<TEntity> DeleteAsync(TEntity entity);


    /// <summary>
    /// Asynchronously marks a range of entities as deleted in the DbSet. This is a soft delete operation.
    /// </summary>
    /// <param name="entities">The list of entities to mark as deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of deleted entities.</returns>
    Task<List<TEntity>> DeleteRangeAsync(List<TEntity> entities);


    /// <summary>
    /// Asynchronously marks entities as deleted based on a predicate. This is a soft delete operation.
    /// </summary>
    /// <param name="predicate">An expression to filter the entities to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    /// Asynchronously marks an entity as deleted based on its identifier. This is a soft delete operation.
    /// </summary>
    /// <param name="id">The identifier of the entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deleted entity, or null if the entity is not found.</returns>
    Task<TEntity> DeleteByIdAsync(TPrimary id);

  
}