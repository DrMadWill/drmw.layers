using DrMW.Repositories.Abstractions.Components.Common.Reads;
using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Concretes.Components.Common.Reads;

public class ReadDatabase<TDbContext> : IReadDatabase
where TDbContext : DbContext
{ 
    public DbContext Context { get; }

    public ReadDatabase(TDbContext context)
    {
        Context = context;
    }
}