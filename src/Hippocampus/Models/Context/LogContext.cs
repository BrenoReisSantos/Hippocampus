using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Models.Context;

public class LogContext : DbContext
{
    public LogContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
       modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogContext).Assembly);
}
