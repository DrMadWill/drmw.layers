using DrMW.Repositories.Abstractions.Components;
using DrMW.Repositories.Abstractions.Components.Common.Reads;
using DrMW.Repositories.Abstractions.Components.Common.Writes;
using DrMW.Repositories.Abstractions.Works;
using DrMW.Repositories.Concretes.Components;
using DrMW.Repositories.Concretes.Components.Common.Reads;
using DrMW.Repositories.Concretes.Components.Common.Writes;
using DrMW.Repositories.Concretes.Works;
using DrMW.Repositories.Services.Abstractions;
using DrMW.Repositories.Services.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DrMW.Repositories;

public static class ServiceRegistration
{
    public static IServiceCollection LayerRepositoriesRegister<TUnitOfWork, TQueryRepositories, TServiceManager,
            TWriteDbContext, TReadDbContext>
        (this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TReadDbContext : DbContext
        where TWriteDbContext : DbContext
        where TUnitOfWork : UnitOfWork<TWriteDbContext>
        where TQueryRepositories : QueryRepositories<TReadDbContext>
        where TServiceManager : ServiceManager

    {

        switch (lifetime)
        {
            case ServiceLifetime.Transient:
                services.AddTransient(typeof(IReadAnonymousRepository<>), typeof(ReadAnonymousRepository<>));
                services.AddTransient(typeof(IWriteAnonymousRepository<>), typeof(WriteAnonymousRepository<>));
                services.AddTransient(typeof(IAnonymousRepository<>), typeof(AnonymousRepository<>));

                services.AddTransient(typeof(IReadOriginRepository<,>), typeof(ReadOriginRepository<,>));
                services.AddTransient(typeof(IWriteOriginRepository<,>), typeof(WriteOriginRepository<,>));
                services.AddTransient(typeof(IOriginRepository<,>), typeof(OriginRepository<,>));

                services.AddTransient(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
                services.AddTransient(typeof(IWriteRepository<,>), typeof(WriteRepository<,>));
                services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));

                break;
            case ServiceLifetime.Singleton:
                services.AddSingleton(typeof(IReadAnonymousRepository<>), typeof(ReadAnonymousRepository<>));
                services.AddSingleton(typeof(IWriteAnonymousRepository<>), typeof(WriteAnonymousRepository<>));
                services.AddSingleton(typeof(IAnonymousRepository<>), typeof(AnonymousRepository<>));

                services.AddSingleton(typeof(IReadOriginRepository<,>), typeof(ReadOriginRepository<,>));
                services.AddSingleton(typeof(IWriteOriginRepository<,>), typeof(WriteOriginRepository<,>));
                services.AddSingleton(typeof(IOriginRepository<,>), typeof(OriginRepository<,>));

                services.AddSingleton(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
                services.AddSingleton(typeof(IWriteRepository<,>), typeof(WriteRepository<,>));
                services.AddSingleton(typeof(IRepository<,>), typeof(Repository<,>));
                break;
            default:
            case ServiceLifetime.Scoped:
                services.AddScoped(typeof(IReadAnonymousRepository<>), typeof(ReadAnonymousRepository<>));
                services.AddScoped(typeof(IWriteAnonymousRepository<>), typeof(WriteAnonymousRepository<>));
                services.AddScoped(typeof(IAnonymousRepository<>), typeof(AnonymousRepository<>));

                services.AddScoped(typeof(IReadOriginRepository<,>), typeof(ReadOriginRepository<,>));
                services.AddScoped(typeof(IWriteOriginRepository<,>), typeof(WriteOriginRepository<,>));
                services.AddScoped(typeof(IOriginRepository<,>), typeof(OriginRepository<,>));

                services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
                services.AddScoped(typeof(IWriteRepository<,>), typeof(WriteRepository<,>));
                services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
                break;

        }

        services.Add(new ServiceDescriptor(typeof(IReadDatabase), typeof(ReadDatabase<TReadDbContext>), lifetime));
        services.Add(new ServiceDescriptor(typeof(IWriteDatabase), typeof(WriteDatabase<TWriteDbContext>), lifetime));
        
        services.Add(new ServiceDescriptor(typeof(IQueryRepositories), typeof(TQueryRepositories), lifetime));
        services.Add(new ServiceDescriptor(typeof(IUnitOfWork), typeof(TUnitOfWork), lifetime));
        services.Add(new ServiceDescriptor(typeof(IServiceManager), typeof(TServiceManager), lifetime));
        

        return services;


    }
}