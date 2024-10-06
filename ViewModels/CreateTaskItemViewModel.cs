using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using ReactiveUI;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels;

public class CreateTaskItemViewModel : ViewModelBase
{
    private string _taskDescription = string.Empty;

    private User? _taskPerformer = null;

    private uint _taskPlannedTime;

    private StatusType _taskStatus = StatusType.None;

    private string _taskTitle = string.Empty;

    private List<User> _users = new();

    public CreateTaskItemViewModel(List<User> users)
    {
        _users = users;
        var canSave = this.WhenAnyValue(t => t.TaskTitle,
                                        t => t.TaskDescription,
                                        t => t.TaskPerformer,
                                        t => t.TaskPlannedTime,
                                        (name, desc, user, time) =>
                                            !string.IsNullOrEmpty(name)
                                            &&
                                            !string.IsNullOrEmpty(desc)
                                            &&
                                            null != user
                                            &&
                                            (0 < time && time <= Constants.s_MaxTimeValue)
                                        );

        SaveCommand = ReactiveCommand.Create<TaskItemViewModel?>(CreateTask, canSave);
    }

    public DateTimeOffset TaskCreatedDate
    {
        get => new DateTimeOffset(DateTime.Now);
    }

    [Required(ErrorMessage = "Description is a required field")]
    public string TaskDescription
    {
        get => _taskDescription;
        set => this.RaiseAndSetIfChanged(ref _taskDescription, value);
    }

    [Required(ErrorMessage = "Performer is a required field")]
    public User TaskPerformer
    {
        get => _taskPerformer!;
        set => this.RaiseAndSetIfChanged(ref _taskPerformer, value);
    }

    [Range(1, Constants.s_MaxTimeValue, ErrorMessage = "Planned time must be between {1} and {2}")]
    public uint TaskPlannedTime
    {
        get => _taskPlannedTime;
        set => this.RaiseAndSetIfChanged(ref _taskPlannedTime, value);
    }

    public ReactiveCommand<Unit, TaskItemViewModel?> SaveCommand { get; }

    public List<StatusType> Statuses
    {
        get => Helper.GetAllowedStatuses(_taskStatus);
    }

    public StatusType TaskStatus
    {
        get => _taskStatus;
        set => this.RaiseAndSetIfChanged(ref _taskStatus, value);
    }

    [Required(ErrorMessage = "Task title is a required field")]
    public string TaskTitle
    {
        get => _taskTitle;
        set => this.RaiseAndSetIfChanged(ref _taskTitle, value);
    }

    public List<User> Users
    {
        get => _users;
    }

    private TaskItemViewModel CreateTask()
    {
        TaskItem task = new TaskItem()
        {
            Title = TaskTitle,
            Description = TaskDescription,
            Status = TaskStatus,
            Performer = TaskPerformer,
            CreatedDate = TaskCreatedDate.DateTime,
            PlanedTimeInHour = TaskPlannedTime
        };
        TaskItemViewModel model = new TaskItemViewModel(task);
        return model;
    }
}
