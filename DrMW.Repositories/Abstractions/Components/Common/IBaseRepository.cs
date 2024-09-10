using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Abstractions.Components.Common;

public interface IBaseRepository<TEntity> : IDisposable
    where TEntity : class
{
    DbSet<TEntity> Table { get; }
}