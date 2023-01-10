using Hippocampus.Models.Values;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Models.Context;

public class LogContext : DbContext
{
    public DbSet<RecipientLog> RecipientLogs => Set<RecipientLog>();

    public LogContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<MacAddress>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogContext).Assembly);
    }
}