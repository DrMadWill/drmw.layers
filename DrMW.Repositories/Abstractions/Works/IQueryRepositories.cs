using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Abstractions.Components.Common.Reads;

namespace DrMW.Repositories.Abstractions.Works;
/// <summary>
/// Base class for query repositories.
/// </summary>
public interface IQueryRepositories : IDisposable
{
    /// <summary>
    /// Gets a repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimary">The primary key type.</typeparam>
    /// <returns>An instance of the read repository for the specified entity type.</returns>
    IReadRepository<TEntity, TPrimary> Repository<TEntity, TPrimary>()
        where TEntity : class, IBaseEntity<TPrimary>;
    
    
    /// <summary>
    /// Gets a origin repository for the specified entity type.
    /// </summary>
    ///<typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimary">The primary key type.</typeparam>
    /// <returns>An instance of the read repository for the specified entity type.</returns>
    IReadOriginRepository<TEntity, TPrimary> OriginRepository<TEntity, TPrimary>()
        where TEntity : class, IOriginEntity<TPrimary>;

    IReadAnonymousRepository<TEntity> AnonymousRepository<TEntity>()
        where TEntity : class;
    /// <summary>
    /// Gets a special repository based on the provided type.
    /// </summary>
    /// <typeparam name="TRepository">The type of the special repository.</typeparam>
    /// <returns>An instance of the special repository.</returns>
    TRepository SpecialRepository<TRepository>();
}