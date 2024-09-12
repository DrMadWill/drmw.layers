using AutoMapper;
using DrMW.Repositories.Abstractions.Works;
using DrMW.Repositories.Services.Abstractions;
using Serilog;

namespace DrMW.Repositories.Services.Concretes
{
    /// <summary>
    /// Base class for service implementations.
    /// </summary>
    public abstract class BaseService : IBaseService
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IQueryRepositories QueryRepositories;
        protected readonly IMapper Mapper;

        /// <summary>
        /// Constructor for BaseService.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance.</param>
        /// <param name="queryRepositories">The query repositories instance.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities.</param>
        protected BaseService(IUnitOfWork unitOfWork, IQueryRepositories queryRepositories, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            QueryRepositories = queryRepositories;
            Mapper = mapper;
        }

        /// <summary>
        /// Executes an asynchronous action and handles exceptions.
        /// </summary>
        /// <param name="action">The asynchronous action to execute.</param>
        /// <returns>True if the action completes successfully; otherwise, false.</returns>
        public async Task<bool> CompleteProcess(Func<Task> action)
        {
            try
            {
                await action();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Log.Error(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Creates an exception with the specified message.
        /// </summary>
        /// <param name="msg">The message for the exception.</param>
        /// <returns>An exception with the specified message.</returns>
        protected virtual Exception CreateException(string msg)
            => new Exception(msg);

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true); ;
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
                UnitOfWork.Dispose();
                QueryRepositories.Dispose();
            }
        }

        /// <summary>
        /// Finalizer to ensure Dispose is called.
        /// </summary>
        ~BaseService()
        {
            Dispose(false);
        }
    }
}
