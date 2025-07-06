using TaskStatus=TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Entities.TaskItems;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public TaskStatus Status { get; set; } = TaskStatus.Created;
}