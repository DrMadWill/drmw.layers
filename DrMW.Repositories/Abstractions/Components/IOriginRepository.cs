using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Abstractions.Components.Common.Reads;
using DrMW.Repositories.Abstractions.Components.Common.Writes;

namespace DrMW.Repositories.Abstractions.Components;

public interface IOriginRepository<TEntity,TPrimary> :IReadOriginRepository<TEntity,TPrimary>,IWriteOriginRepository<TEntity,TPrimary>
    where TEntity : class, IOriginEntity<TPrimary>

{
}