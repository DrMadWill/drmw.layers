using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Abstractions.Components.Common.Writes;

public interface IWriteDatabase
{
    DbContext Context { get; }
}