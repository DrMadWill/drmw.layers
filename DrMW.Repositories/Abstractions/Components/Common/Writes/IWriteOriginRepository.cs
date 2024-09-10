using DrMW.Core.Models.Abstractions;

namespace DrMW.Repositories.Abstractions.Components.Common.Writes;

public interface IWriteOriginRepository<TEntity, in TPrimary> : IWriteAnonymousRepository<TEntity>
    where TEntity : class, IOriginEntity<TPrimary>

{
 
}