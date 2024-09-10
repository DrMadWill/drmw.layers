using System.Reflection;
using AutoMapper;
using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Abstractions.Components;
using DrMW.Repositories.Abstractions.Works;
using DrMW.Repositories.Concretes.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DrMW.Repositories.Concretes.Works
{
    /// <summary>
    /// Base class for unit of work implementations.
    /// </summary>
    public abstract class UnitOfWork<TDbContext> : IUnitOfWork
     where TDbContext : DbContext
    {
        protected readonly TDbContext DbContext;
        protected readonly Dictionary<Type, object> Repositories;
        protected readonly Assembly Assembly;
        protected readonly IMapper Mapper;
        protected readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// Constructor for UnitOfWork.
        /// </summary>
        /// <param name="context">The DbContext to be used for unit of work.</param>
        /// <param name="assembly">The Type used for assembly information.</param>
        /// <param name="mapper">The IMapper for mapping class</param>
        /// <param name="serviceProvider"></param>
        public UnitOfWork(TDbContext context, Assembly assembly, IMapper mapper, IServiceProvider serviceProvider)
        {
            DbContext = context;
            Repositories = new Dictionary<Type, object>();
            Assembly = assembly;
            Mapper = mapper;
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets a write repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimary">The primary key type.</typeparam>
        /// <returns>An instance of the write repository for the specified entity type.</returns>
        public virtual IRepository<TEntity, TPrimary> Repository<TEntity, TPrimary>()
            where TEntity : class, IBaseEntity<TPrimary>
        {
            if (Repositories.TryGetValue(typeof(TEntity), out var repository))
                return repository as IRepository<TEntity, TPrimary>;

            var repo = new Repository<TEntity, TPrimary>(DbContext,Mapper);
            Repositories.Add(typeof(TEntity), repo);
            return repo;
        }

        public IAnonymousRepository<TEntity> AnonymousRepository<TEntity>() where TEntity : class
        {
            if (Repositories.TryGetValue(typeof(TEntity), out var repository))
                return repository as IAnonymousRepository<TEntity>;

            var repo = new AnonymousRepository<TEntity>(DbContext);
            Repositories.Add(typeof(TEntity), repo);
            return repo;
        }

        public IOriginRepository<TEntity, TPrimary> OriginRepository<TEntity, TPrimary>() where TEntity : class, IOriginEntity<TPrimary>
        {
            if (Repositories.TryGetValue(typeof(TEntity), out var repository))
                return repository as IOriginRepository<TEntity, TPrimary>;

            var repo = new OriginRepository<TEntity, TPrimary>(DbContext,Mapper);
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
            if (Repositories.TryGetValue(typeof(TRepository), out var repository))
                return (TRepository)repository;

            var implementationType = Assembly.GetTypes()
                .FirstOrDefault(x => !x.IsAbstract
                                     && !x.IsInterface
                                     && x.Name == typeof(TRepository).Name.Substring(1));

            if (implementationType == null)
                throw new KeyNotFoundException($"Repository type is not found: {typeof(TRepository).Name.Substring(1)}");

            var instance = (TRepository)ActivatorUtilities.CreateInstance(ServiceProvider, implementationType, DbContext);
            if (instance is not TRepository typedRepository)
                throw new InvalidOperationException($"Created instance is not of type {typeof(TRepository)}");
            
            Repositories.Add(typeof(TRepository), typedRepository);

            return typedRepository;
        }

        /// <summary>
        /// Asynchronously commits changes to the DbContext.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual Task CommitAsync() => DbContext.SaveChangesAsync();

        public virtual async Task SynchronizationData<TEvent, TEntity, TPrimary>(TEvent @event,Action<string>? log = null) 
            where TEvent : class, IHasDelete 
            where TEntity : class, IOriginEntity<TPrimary>, new()
        {
            var repo = OriginRepository<TEntity, TPrimary>();
            var dict = Mapper.Map<TEntity>(@event);
            if (@event.IsDeleted == true)
            {
                try
                {
                    await repo.RemoveAsync(dict);
                    await CommitAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    log?.Invoke(nameof(TEvent) + $" error occur | not deleted | err : {e}");
                }
            }
            else
            {
                if (await repo.Table.AnyAsync(s => s.Id.Equals(dict.Id))) await repo.UpdateAsync(dict);
                else await repo.AddAsync(dict);
                await CommitAsync();
            }
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
        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
