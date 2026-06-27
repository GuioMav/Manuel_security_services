using System;

namespace SecurityAwarenessBot.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? ReminderDate { get; set; }
    public bool HasReminder => ReminderDate.HasValue;
    public bool IsCompleted { get; set; }
}
