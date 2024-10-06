using System;
using Avalonia.Media;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels;

public class TaskItemViewModel : ViewModelBase
{
    private readonly TaskItem _task;

    public IBrush StatusBrush
    {
        get => Helper.GetBrush(_task.Status);
    }

    public TaskItem Task
    {
        get => _task;
    }

    public string TaskCreatedDate
    {
        get => _task.CreatedDate.ToShortDateString();
    }

    public string TaskDescription
    {
        get => _task.Description;
    }

    public int TaskId
    {
        get => _task.Id;
    }

    public User TaskPerformer
    {
        get => _task.Performer;
    }

    public uint TaskPlanedTime
    {
        get => _task.PlanedTimeInHour;
    }

    public uint TaskSpendTime
    {
        get => _task.SpendTimeInHour;
    }

    public string TaskStatus
    {
        get => _task.Status.ToString();
    }

    public string TaskTitle
    {
        get => _task.Title;
    }

    public TaskItemViewModel(TaskItem task)
    {
        if (null == task)
        {
            throw new ArgumentException(nameof(task));
        }

        _task = task;
    }
}
