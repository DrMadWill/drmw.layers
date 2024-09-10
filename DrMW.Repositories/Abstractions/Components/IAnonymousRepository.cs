using DrMW.Repositories.Abstractions.Components.Common.Reads;
using DrMW.Repositories.Abstractions.Components.Common.Writes;

namespace DrMW.Repositories.Abstractions.Components;
  
public interface IAnonymousRepository<TEntity> : IReadAnonymousRepository<TEntity>,IWriteAnonymousRepository<TEntity>
    where TEntity  : class
{
    
}