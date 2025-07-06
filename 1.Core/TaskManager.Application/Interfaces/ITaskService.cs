using TaskManager.Domain.Entities.TaskItems;

namespace TaskManager.Application.Interfaces;
public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem> CreateAsync(string title, string? description);
    Task<bool> MarkAsDoneAsync(Guid id);
    Task<bool> CancelAsync(Guid id);
}
