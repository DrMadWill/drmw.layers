using DrMW.Core.Models.Abstractions;

namespace DrMW.Repositories.Abstractions.Components.Common.Reads;
/// <summary>
/// Represents a generic read-only repository for accessing entities of type TEntity with primary key of type TPrimary.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TPrimary">The type of the primary key for the entity.</typeparam>
public interface IReadRepository<TEntity, TPrimary> : IReadOriginRepository<TEntity,TPrimary>
    where TEntity : class, IBaseEntity<TPrimary>
{
    /// <summary>
    /// Retrieves an IQueryable for all entities of type TEntity, optionally including deleted entities and/or applying tracking.
    /// </summary>
    /// <param name="ordering"></param>
    /// <returns>An IQueryable of all entities of type TEntity.</returns>
    IQueryable<TEntity> Queryable(byte ordering = 1);



}