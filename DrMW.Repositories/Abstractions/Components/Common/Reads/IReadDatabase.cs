using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Abstractions.Components.Common.Reads;

public interface IReadDatabase
{
    DbContext Context { get; }
}