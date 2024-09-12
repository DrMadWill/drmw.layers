using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Abstractions.Components.Common.Writes;
using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Concretes.Components.Common.Writes;

public class WriteOriginRepository<TEntity, TPrimary> :  WriteAnonymousRepository<TEntity>,IWriteOriginRepository<TEntity, TPrimary>
    where TEntity : class, IOriginEntity<TPrimary>, new()
{

    /// <summary>
    /// Initializes a new instance of the <see cref="WriteOriginRepository{TEntity,TPrimary}"/> class.
    /// </summary>
    /// <param name="database">The database context to be used b y the repository.</param>
    public WriteOriginRepository(IWriteDatabase database):base(database)
    {
    }
    
    protected   internal WriteOriginRepository(DbContext dbContext):base(dbContext)
    {
    }

   
    
    /// <summary>
    /// Destructor for WriteRepository.
    /// </summary>
    ~WriteOriginRepository()
    {
        Dispose(false);
    }

}