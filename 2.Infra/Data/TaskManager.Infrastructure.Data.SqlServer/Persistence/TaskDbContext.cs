using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.TaskItems;
using TaskManager.Infrastructure.Data.SqlServer.Persistence.Configurations;

namespace TaskManager.Infrastructure.Data.SqlServer.Persistence;
public class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TaskItemConfiguration());
    }
}
