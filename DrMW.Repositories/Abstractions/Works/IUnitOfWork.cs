using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Abstractions.Components;

namespace DrMW.Repositories.Abstractions.Works;
/// <summary>
/// Base class for unit of work implementations.
/// </summary>
public interface IUnitOfWork : IDisposable
{

    /// <summary>
    /// Gets a write repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>An instance of the write repository for the specified entity type.</returns>
    IAnonymousRepository<TEntity> AnonymousRepository<TEntity>() where TEntity : class;
   


    /// <summary>
    /// Gets a origin write repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimary">The primary key type.</typeparam>
    /// <returns>An instance of the write repository for the specified entity type.</returns>
    IOriginRepository<TEntity, TPrimary> OriginRepository<TEntity, TPrimary>()
        where TEntity : class, IOriginEntity<TPrimary>;
    
    
    /// <summary>
    /// Gets a write repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimary">The primary key type.</typeparam>
    /// <returns>An instance of the write repository for the specified entity type.</returns>
    IRepository<TEntity, TPrimary> Repository<TEntity, TPrimary>()
        where TEntity : class, IBaseEntity<TPrimary>;
    
    /// <summary>
    /// Gets a special repository based on the provided type.
    /// </summary>
    /// <typeparam name="TRepository">The type of the special repository.</typeparam>
    /// <returns>An instance of the special repository.</returns>
    TRepository SpecialRepository<TRepository>();

    /// <summary>
    /// Asynchronously commits changes to the DbContext.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task CommitAsync();

    Task SynchronizationData<TEvent, TEntity, TPrimary>(TEvent @event, Action<string>? log = null)
        where TEvent : class, IHasDelete
        where TEntity : class, IOriginEntity<TPrimary>, new();

}