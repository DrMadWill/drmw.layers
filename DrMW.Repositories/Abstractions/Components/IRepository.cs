using System.Linq.Expressions;
using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Abstractions.Components.Common.Reads;
using DrMW.Repositories.Abstractions.Components.Common.Writes;

namespace DrMW.Repositories.Abstractions.Components;
/// <summary>
/// Represents a generic repository for write operations on entities of type TEntity with primary key of type TPrimary.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TPrimary">The type of the primary key for the entity.</typeparam>
public interface IRepository<TEntity, TPrimary> :IReadRepository<TEntity,TPrimary>, IWriteRepository<TEntity,TPrimary>
    where TEntity : class, IBaseEntity<TPrimary>

{
}