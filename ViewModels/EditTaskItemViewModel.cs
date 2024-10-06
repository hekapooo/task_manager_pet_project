using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using DynamicData.Binding;
using ReactiveUI;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels;

public class EditTaskItemViewModel : ViewModelBase
{
    private readonly List<StatusType> _allowedStatuses;

    private DateTime _taskCompleteDate;

    private string _taskDescription;

    private User _taskPerformer;

    private uint _taskSpendTime;

    private StatusType _taskStatus;

    private string _taskTitle;

    private TaskItemViewModel _taskVm;

    private readonly List<User> _users = new();

    public EditTaskItemViewModel(TaskItemViewModel taskVm, List<User> users)
    {
        if (null == taskVm)
        {
            throw new ArgumentException(nameof(taskVm));
        }

        _allowedStatuses = Helper.GetAllowedStatuses(taskVm.Task.Status);
        _taskCompleteDate = taskVm.Task.CompletedDate;
        _taskDescription = taskVm.Task.Description;
        _taskPerformer = taskVm.Task.Performer;
        _taskSpendTime = taskVm.Task.SpendTimeInHour;
        _taskStatus = taskVm.Task.Status;
        _taskTitle = taskVm.Task.Title;
        _taskVm = taskVm;
        _users = users;

        this.WhenAnyPropertyChanged().Subscribe(_ => IsModified = true);

        var canSave = this.WhenAnyValue(t => t.TaskSpendTime,
                                        (time) =>
                                            (0 < time && time <= Constants.s_MaxTimeValue)
                                        );

        SaveCommand = ReactiveCommand.Create<TaskItemViewModel?>(EditTask, canSave);
    }

    public List<StatusType> AllowedStatuses
    {
        get => _allowedStatuses;
    }

    public bool IsModified { get; private set; } = false;

    public ReactiveCommand<Unit, TaskItemViewModel?> SaveCommand { get; }

    public DateTimeOffset TaskCompleteDate
    {
        get
        {
            if (!_taskCompleteDate.Equals(DateTime.MinValue))
            {
                return new DateTimeOffset(_taskCompleteDate);
            }
            else
            {
                return new DateTimeOffset(DateTime.MinValue.AddYears(DateTime.Now.Year - 1));
            }
        }
    }

    public DateTimeOffset TaskCreatedDate
    {
        get => new DateTimeOffset(_taskVm.Task.CreatedDate);
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
        get => _taskPerformer;
        set => this.RaiseAndSetIfChanged(ref _taskPerformer, value);
    }

    public uint TaskPlannedTime
    {
        get => _taskVm.Task.PlanedTimeInHour;
    }

    [Range(1, Constants.s_MaxTimeValue, ErrorMessage = "Spend time must be between {1} and {2}")]
    public uint TaskSpendTime
    {
        get => _taskSpendTime;
        set => this.RaiseAndSetIfChanged(ref _taskSpendTime, value);
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

    private TaskItemViewModel EditTask()
    {
        if (!IsModified)
        {
            return _taskVm;
        }

        _taskVm.Task.Title = TaskTitle;
        _taskVm.Task.Description = TaskDescription;
        _taskVm.Task.Status = TaskStatus;
        if (TaskStatus.Equals(StatusType.Completed))
        {
            _taskVm.Task.CompletedDate = DateTime.Now;
        }
        _taskVm.Task.Performer = TaskPerformer;
        _taskVm.Task.SpendTimeInHour = TaskSpendTime;
        return _taskVm;
    }
}
