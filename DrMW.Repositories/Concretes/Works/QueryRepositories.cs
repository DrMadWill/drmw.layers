using System.Reflection;
using AutoMapper;
using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Abstractions.Components;
using DrMW.Repositories.Abstractions.Components.Common.Reads;
using DrMW.Repositories.Abstractions.Works;
using DrMW.Repositories.Concretes.Components.Common.Reads;
using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Concretes.Works
{
    /// <summary>
    /// Base class for query repositories.
    /// </summary>
    public abstract class QueryRepositories<TDbContext> : IQueryRepositories, IDisposable
        where TDbContext : DbContext
    {
        protected readonly TDbContext DbContext;
        protected readonly IMapper Mapper;
        protected readonly Dictionary<Type, object> Repositories;
        protected readonly Assembly Assembly;

        /// <summary>
        /// Constructor for QueryRepositories.
        /// </summary>
        /// <param name="dbContext">The DbContext to be used for queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities.</param>
        /// <param name="assembly">The Type used for assembly information.</param>
        public QueryRepositories(TDbContext dbContext, IMapper mapper, Assembly assembly)
        {
            DbContext = dbContext;
            Mapper = mapper;
            Repositories = new Dictionary<Type, object>();
            Assembly = assembly;
        }
        
        public IReadAnonymousRepository<TEntity> AnonymousRepository<TEntity>() where TEntity : class
        {
            if (Repositories.TryGetValue(typeof(TEntity), out var repository))
                return repository as IAnonymousRepository<TEntity>;
           
            var repo = new ReadAnonymousRepository<TEntity>(DbContext);
            Repositories.Add(typeof(TEntity), repo);
            return repo; 
        }

        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimary">The primary key type.</typeparam>
        /// <returns>An instance of the read repository for the specified entity type.</returns>
        public virtual IReadRepository<TEntity, TPrimary> Repository<TEntity, TPrimary>()
            where TEntity : class, IBaseEntity<TPrimary>
        {
            if (Repositories.TryGetValue(typeof(TEntity), out var repository))
                return repository as IReadRepository<TEntity, TPrimary>;
            
            var repo = new ReadRepository<TEntity, TPrimary>(DbContext, Mapper);
            Repositories.Add(typeof(TEntity), repo);
            return repo;
        }

        public IReadOriginRepository<TEntity, TPrimary> OriginRepository<TEntity, TPrimary>() where TEntity : class, IOriginEntity<TPrimary>
        {
            if (Repositories.TryGetValue(typeof(TEntity), out var repository))
                return repository as IReadOriginRepository<TEntity, TPrimary>;

            var repo = new ReadOriginRepository<TEntity, TPrimary>(DbContext, Mapper);
            Repositories.Add(typeof(TEntity), repo);
            return repo; 
        }

        

        /// <summary>
        /// Gets a special repository based on the provided type.
        /// </summary>
        /// <typeparam name="TRepository">The type of the special repository.</typeparam>
        /// <returns>An instance of the special repository.</returns>
        public virtual TRepository SpecialRepository<TRepository>()
        {
            if (Repositories.Keys.Contains(typeof(TRepository)))
                return (TRepository)Repositories[typeof(TRepository)];

            var type = Assembly.GetTypes()
                .FirstOrDefault(x => !x.IsAbstract
                                     && !x.IsInterface
                                     && x.Name == typeof(TRepository).Name.Substring(1));

            if (type == null)
                throw new KeyNotFoundException($"Repository type is not found: {typeof(TRepository).Name.Substring(1)}");

            var repository = (TRepository)Activator.CreateInstance(type, DbContext, Mapper)!;

            Repositories.Add(typeof(TRepository), repository);

            return repository;
        }

        /// <summary>
        /// Disposes of the DbContext and clears repository references.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        /// <param name="disposing">True if disposing; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose of DbContext
                DbContext?.Dispose();

                // Dispose of repositories that implement IDisposable
                foreach (var repository in Repositories.Values)
                {
                    if (repository is IDisposable disposableRepo)
                    {
                        disposableRepo.Dispose();
                    }
                }
                Repositories.Clear();
            }
        }

        /// <summary>
        /// Finalizer to ensure Dispose is called.
        /// </summary>
        ~QueryRepositories()
        {
            Dispose(false);
        }
    }
}
