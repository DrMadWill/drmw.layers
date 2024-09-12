using System.Linq.Expressions;
using DrMW.Repositories.Abstractions.Components.Common.Reads;
using DrMW.Repositories.Extensions.Paging;
using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Concretes.Components.Common.Reads;

public class ReadAnonymousRepository<TEntity> : BaseRepository<TEntity>,IReadAnonymousRepository<TEntity> 
    where TEntity : class
{

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="dbContext">The database context to be used by the repository.</param>
    public ReadAnonymousRepository(DbContext dbContext) : base(dbContext)
    {
    }
    
    /// <summary>
    /// Binds include properties to the given query.
    /// </summary>
    /// <param name="query">The query to which the include properties will be added.</param>
    /// <param name="includeProperties">A collection of expressions indicating the properties to be included in the query.</param>
    protected void BindIncludeProperties(IQueryable<TEntity> query,
        IEnumerable<Expression<Func<TEntity, object>>> includeProperties)
    {
        includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    /// <summary>
    /// Retrieves an IQueryable for all entities of type TEntity, optionally including deleted entities and/or applying tracking.
    /// </summary>
    /// <returns>An IQueryable of all entities of type TEntity.</returns>
    public virtual IQueryable<TEntity> Queryable(bool isTracking = false) =>
        isTracking ? (Table.AsNoTracking()) : Table.AsQueryable();

    /// <summary>
    /// Retrieves an IQueryable for all entities of type TEntity, optionally including deleted entities and/or applying tracking.
    /// </summary>
    /// <returns>An IQueryable of all entities of type TEntity.</returns>
    public virtual IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate) =>
        Queryable().Where(predicate);
    
    
    
    /// <summary>
    /// Retrieves an IQueryable for all entities of type TEntity, including specified related entities.
    /// </summary>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <returns>An IQueryable of all entities of type TEntity with specified related entities included.</returns>
    public virtual IQueryable<TEntity> IncludingQueryable(
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = Queryable();
        BindIncludeProperties(query, includeProperties);
        includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return query;
    }

    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity as a list.
    /// </summary>
    ///  <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all entities of type TEntity.</returns>
    public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate) =>
        Queryable(predicate).ToListAsync();


    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity including specified related entities and returns them as a list.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all entities of type TEntity with specified related entities included.</returns>
    public virtual Task<List<TEntity>> GetAllListIncludingAsync(Expression<Func<TEntity, bool>> predicate,params Expression<Func<TEntity, object>>[] includeProperties) 
        => IncludingQueryable(includeProperties).Where(predicate).ToListAsync();
    
    

    /// <summary>
    /// Asynchronously retrieves the first entity of type TEntity that satisfies a specified condition.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains the first entity that satisfies the condition, or null if no such entity is found.</returns>
    public virtual Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate) =>
        Queryable().FirstOrDefaultAsync(predicate);

    /// <summary>
    /// Retrieves an IQueryable for entities of type TEntity that satisfy a specified condition.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>An IQueryable of entities that satisfy the specified condition.</returns>
    public virtual IQueryable<TEntity> FindByQueryable(Expression<Func<TEntity, bool>> predicate) =>
        Queryable().Where(predicate);


    /// <summary>
    /// Retrieves an IQueryable for entities of type TEntity that satisfy a specified condition, including specified related entities.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <returns>An IQueryable of entities that satisfy the specified condition with specified related entities included.</returns>
    public virtual IQueryable<TEntity> FindByIncludingQueryable(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = Queryable();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return query.Where(predicate);
    }

    /// <summary>
    /// Asynchronously determines whether any entity of type TEntity satisfies a condition.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>A Task representing the asynchronous operation. The task result is true if any entities satisfy the condition; otherwise, false.</returns>
    public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        => Queryable().AnyAsync(predicate);


    /// <summary>
    /// Asynchronously determines whether all entities of type TEntity satisfy a condition.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>A Task representing the asynchronous operation. The task result is true if all entities satisfy the condition; otherwise, false.</returns>
    public virtual Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate)
        => Queryable().AllAsync(predicate);

    /// <summary>
    /// Asynchronously counts the number of entities of type TEntity.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation. The task result contains the number of entities.</returns>
    public virtual Task<int> CountAsync()
        => Queryable().CountAsync();

    /// <summary>
    /// Asynchronously counts the number of entities of type TEntity satisfying a specified condition.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains the number of entities that satisfy the condition.</returns>
    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        => Queryable().CountAsync(predicate);




    /// <summary>
    /// IQueryable | Asynchronously retrieves a paged result from a given IQueryable source.
    /// </summary>
    /// <param name="source">The IQueryable source to paginate.</param>
    /// <param name="req">The pagination request details.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of entities.</returns>
    public virtual async Task<SourcePaged<TEntity>> GetSourcePagedAsync(IQueryable<TEntity> source, PageReq req)
        => await SourcePaged<TEntity>.PagedAsync(source, req);

    /// <summary>
    /// Predicate | Asynchronously retrieves a paged result for entities of type TEntity that satisfy a given predicate and includes specified related entities.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <param name="req">The pagination request details.</param>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of entities that satisfy the condition.</returns>
    public virtual async Task<SourcePaged<TEntity>> GetSourcePagedAsync(Expression<Func<TEntity, bool>> predicate,
        PageReq req, params Expression<Func<TEntity, object>>[] includeProperties)
        => await GetSourcePagedAsync(IncludingQueryable(includeProperties).Where(predicate), req);


    

    /// <summary>
    /// Query Search | Asynchronously retrieves a paged result using a custom query function applied to the entities of type TEntity.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="func">A function to transform the IQueryable before pagination is applied.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of entities after applying the custom query function.</returns>
    public virtual async Task<SourcePaged<TEntity>> GetSourcePagedFuncQueryAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func,PageReq req)
        => await GetSourcePagedAsync(func == null ? Queryable() : func(Queryable()), req);

  
    
    /// <summary>
    /// Func | Asynchronously retrieves a paged result using a custom function applied to a list of entities of type TEntity.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="func">A function to transform the list of entities before pagination is applied.</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of entities after applying the custom function.</returns>
    public virtual async Task<SourcePaged<TEntity>> GetSourcePagedEndFuncAsync(PageReq req,
        Func<List<TEntity>, List<TEntity>>? func = null)
    {
        var source = Queryable();
        req.Page = req.Page == 0 ? 1 : req.Page;

        if (req.PerPage > 0 && req.PerPage <= 200)
            Paginate<TEntity>.PerPage = req.PerPage;

        return new SourcePaged<TEntity>
        {
            PagingModel = new PageModel(await source.CountAsync(), req.Page, Paginate<TEntity>.PerPage),
            Source = func == null
                ? await Paginate<TEntity>.Paging(source, req.Page).ToListAsync()
                : func((await Paginate<TEntity>.Paging(source, req.Page).ToListAsync())),
        };
    }


     /// <summary>
    /// Destructor for AnonymousReadRepository.
    /// </summary>
    ~ReadAnonymousRepository()
    {
        Dispose(false);
    }
    
}