using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities.TaskItems;
using TaskManager.Infrastructure.Data.SqlServer.Persistence;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Application.Services;

public class TaskService(TaskDbContext context) : ITaskService
{
    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await context.Tasks
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<TaskItem> CreateAsync(string title, string? description)
    {
        var task = new TaskItem
        {
            Title = title,
            Description = description,
            Status = TaskStatus.Created,
            CreatedAt = DateTime.UtcNow
        };

        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        return task;
    }

    public async Task<bool> MarkAsDoneAsync(Guid id)
    {
        var task = await context.Tasks.FindAsync(id);
        if (task == null || task.Status != TaskStatus.Created)
            return false;

        task.Status = TaskStatus.Done;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelAsync(Guid id)
    {
        var task = await context.Tasks.FindAsync(id);
        if (task == null || task.Status != TaskStatus.Created)
            return false;

        task.Status = TaskStatus.Canceled;
        await context.SaveChangesAsync();
        return true;
    }
}
