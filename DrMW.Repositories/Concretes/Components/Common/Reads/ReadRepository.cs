using System.Linq.Expressions;
using AutoMapper;
using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Abstractions.Components.Common.Reads;
using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Concretes.Components.Common.Reads;

/// <summary>
/// Represents a generic read-only repository for accessing entities of type TEntity with primary key of type TPrimary.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TPrimary">The type of the primary key for the entity.</typeparam>
public class ReadRepository<TEntity, TPrimary> : ReadOriginRepository<TEntity,TPrimary>,IReadRepository<TEntity, TPrimary>
    where TEntity : class, IBaseEntity<TPrimary>
{
  
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadRepository{TEntity,TPrimary}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context to be used by the repository.</param>
    /// <param name="mapper">The AutoMapper instance for entity-DTO mappings.</param>
    public ReadRepository(DbContext dbContext, IMapper mapper) : base(dbContext,mapper)
    {
    }

    public override IQueryable<TEntity> Queryable(bool isTracking = false) =>
        base.Queryable(isTracking).Where(s => s.IsDeleted != true)
            .OrderByDescending(s => s.CreatedDate);
    
    public IQueryable<TEntity> Queryable(byte ordering = 1)
        => ordering == 1 ? Queryable(false) : Table.AsNoTracking().Where(s => s.IsDeleted != true);
    
    /// <summary>
    /// Destructor for ReadOriginRepository.
    /// </summary>
    ~ReadRepository()
    {
        Dispose(false);
    }
}