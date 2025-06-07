using System.Linq.Expressions;
using Dr.Pagination;
using DrMW.Repositories.Extensions.Paging;

namespace DrMW.Repositories.Abstractions.Components.Common.Reads;
  
public interface IReadAnonymousRepository<TEntity> : IBaseRepository<TEntity> 
    where TEntity  : class
{
    /// <summary>
    /// Retrieves an IQueryable for all entities of type TEntity
    /// </summary>
    /// <returns>An IQueryable of all entities of type TEntity.</returns>
    IQueryable<TEntity> Queryable(bool isTracking = false);
    
     /// <summary>
    /// Retrieves an IQueryable for all entities of type TEntity
    /// </summary>
    /// <returns>An IQueryable of all entities of type TEntity.</returns>
    IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Retrieves an IQueryable for all entities of type TEntity, including specified related entities.
    /// </summary>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <returns>An IQueryable of all entities of type TEntity with specified related entities included.</returns>
    IQueryable<TEntity> IncludingQueryable(params Expression<Func<TEntity, object>>[] includeProperties);

    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity as a list.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all entities of type TEntity.</returns>
    Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity including specified related entities and returns them as a list.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all entities of type TEntity with specified related entities included.</returns>
    Task<List<TEntity>> GetAllListIncludingAsync(Expression<Func<TEntity, bool>> predicate,params Expression<Func<TEntity, object>>[] includeProperties);

    /// <summary>
    /// Asynchronously retrieves the first entity of type TEntity that satisfies a specified condition.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains the first entity that satisfies the condition, or null if no such entity is found.</returns>
    Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Retrieves an IQueryable for entities of type TEntity that satisfy a specified condition.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>An IQueryable of entities that satisfy the specified condition.</returns>
    IQueryable<TEntity> FindByQueryable(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    /// Retrieves an IQueryable for entities of type TEntity that satisfy a specified condition, including specified related entities.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <returns>An IQueryable of entities that satisfy the specified condition with specified related entities included.</returns>
    IQueryable<TEntity> FindByIncludingQueryable(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    /// <summary>
    /// Asynchronously determines whether any entity of type TEntity satisfies a condition.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>A Task representing the asynchronous operation. The task result is true if any entities satisfy the condition; otherwise, false.</returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    /// Asynchronously determines whether all entities of type TEntity satisfy a condition.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>A Task representing the asynchronous operation. The task result is true if all entities satisfy the condition; otherwise, false.</returns>
    Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Asynchronously counts the number of entities of type TEntity.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation. The task result contains the number of entities.</returns>
    Task<int> CountAsync();

    /// <summary>
    /// Asynchronously counts the number of entities of type TEntity satisfying a specified condition.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains the number of entities that satisfy the condition.</returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// IQueryable | Asynchronously retrieves a paged result from a given IQueryable source.
    /// </summary>
    /// <param name="source">The IQueryable source to paginate.</param>
    /// <param name="req">The pagination request details.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of entities.</returns>
    Task<SourcePaged<TEntity>> GetSourcePagedAsync(IQueryable<TEntity> source, PageReq req);

    /// <summary>
    /// Predicate | Asynchronously retrieves a paged result for entities of type TEntity that satisfy a given predicate and includes specified related entities.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <param name="req">The pagination request details.</param>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of entities that satisfy the condition.</returns>
    Task<SourcePaged<TEntity>> GetSourcePagedAsync(Expression<Func<TEntity, bool>> predicate, PageReq req, params Expression<Func<TEntity, object>>[] includeProperties);

    /// <summary>
    /// Query Search | Asynchronously retrieves a paged result using a custom query function applied to the entities of type TEntity.
    /// </summary>
    /// <param name="func">A function to transform the IQueryable before pagination is applied.</param>
    /// <param name="req">The pagination request details.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of entities after applying the custom query function.</returns>
    Task<SourcePaged<TEntity>> GetSourcePagedFuncQueryAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func,PageReq req);

    /// <summary>
    /// Func | Asynchronously retrieves a paged result using a custom function applied to a list of entities of type TEntity.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="func">A function to transform the list of entities before pagination is applied.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of entities after applying the custom function.</returns>
    Task<SourcePaged<TEntity>> GetSourcePagedEndFuncAsync(PageReq req, Func<List<TEntity>, List<TEntity>>? func = null);
}