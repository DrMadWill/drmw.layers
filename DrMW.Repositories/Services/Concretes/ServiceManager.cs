using System.Reflection;
using AutoMapper;
using DrMW.Repositories.Abstractions.Works;
using DrMW.Repositories.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DrMW.Repositories.Services.Concretes
{
    /// <summary>
    /// Base class for managing services.
    /// </summary>
    public class ServiceManager : IServiceManager
    {
        protected readonly Dictionary<Type, object> Services;
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IQueryRepositories QueryRepositories;
        protected readonly IMapper Mapper;
        protected readonly Assembly Assembly;
        protected readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// Constructor for ServiceManager.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance.</param>
        /// <param name="queryRepositories">The query repositories instance.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities.</param>
        /// <param name="assembly">The Type used for assembly information.</param>
        /// <param name="serviceProvider"></param>
        public ServiceManager(IUnitOfWork unitOfWork, IQueryRepositories queryRepositories, IMapper mapper, 
            Assembly assembly, IServiceProvider serviceProvider)
        {
            UnitOfWork = unitOfWork;
            QueryRepositories = queryRepositories;
            Mapper = mapper;
            Services = new Dictionary<Type, object>();
            Assembly = assembly;
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets a service of the specified type.
        /// </summary>
        /// <typeparam name="TService">The type of service to retrieve.</typeparam>
        /// <returns>An instance of the specified service type.</returns>
        public virtual TService Service<TService>()
        {
            if (Services.ContainsKey(typeof(TService)))
                return (TService)Services[typeof(TService)];

            var type = Assembly.GetTypes()
                .FirstOrDefault(x => !x.IsAbstract
                                     && !x.IsInterface
                                     && x.Name == typeof(TService).Name.Substring(1));

            if (type == null)
                throw new KeyNotFoundException($"Service type is not found. Service Name: {typeof(TService).Name.Substring(1)}");

            var service = (TService)ActivatorUtilities.CreateInstance(ServiceProvider, type, UnitOfWork,QueryRepositories,Mapper);

            Services.Add(typeof(TService), service);

            return service;
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        public void Dispose()
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
            if (!disposing) return;
            // Dispose of services that implement IDisposable
            foreach (var service in Services.Values)
            {
                if (service is IDisposable disposableService)
                {
                    disposableService.Dispose();
                }
            }

            Services.Clear();
        }

        /// <summary>
        /// Finalizer to ensure Dispose is called.
        /// </summary>
        ~ServiceManager()
        {
            Dispose(false);
        }
    }
}
