using System;

namespace TaskManager.Models;

public class TaskItem : IDomainObject
{
    public DateTime CompletedDate { get; set; }

    public required DateTime CreatedDate { get; set; }

    public required string Description { get; set; }

    public int Id { get; set; }

    public required User Performer { get; set; }

    public required uint PlanedTimeInHour { get; set; }

    public uint SpendTimeInHour { get; set; }

    public required StatusType Status { get; set; }

    public required string Title { get; set; }
}
