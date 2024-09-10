using DrMW.Repositories.Abstractions.Components.Common;
using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Concretes.Components.Common;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
where TEntity : class
{
    protected readonly DbContext AppDbContext;
    protected BaseRepository(DbContext dbContext)
    {
        AppDbContext = dbContext;
        Table = dbContext.Set<TEntity>();
    }
    public DbSet<TEntity> Table { get; }
 
    
    
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Protected implementation of Dispose pattern.
    /// </summary>
    /// <param name="disposing">Indicates whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Free any other managed objects here.
        }
    }

    /// <summary>
    /// Destructor for ReadRepository.
    /// </summary>
    ~BaseRepository()
    {
        Dispose(false);
    }
}