using DrMW.Repositories.Abstractions.Components.Common.Writes;
using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Concretes.Components.Common.Writes;

public class WriteDatabase<TDbContext> : IWriteDatabase
where TDbContext : DbContext
{
    public DbContext Context { get; }

    public WriteDatabase(TDbContext context)
    {
        Context = context;
    }
}