using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private bool _isBusy = false;

    private bool _isElementsVisible = false;

    private IRepository _repository;

    public Interaction<ViewModelBase, ViewModelBase?> CreateDialog { get; }

    public ICommand CreateCommand { get; }

    public Interaction<ViewModelBase, ViewModelBase?> EditDialog { get; }

    public ICommand EditCommand { get; }

    public bool IsElementsVisible
    {
        get => _isElementsVisible;
        set => this.RaiseAndSetIfChanged(ref _isElementsVisible, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    public ICommand RemoveCommand { get; }

    public TaskItemViewModel? SelectedItem { get; set; }

    public ObservableCollection<TaskItemViewModel> Tasks { get; } = new();

    public List<User> Users { get; } = new();

    public MainWindowViewModel(IRepository repository)
    {
        _repository = repository;
        CreateDialog = new Interaction<ViewModelBase, ViewModelBase?>();
        EditDialog = new Interaction<ViewModelBase, ViewModelBase?>();

        var removeOk = this.WhenAny(
                        x => x.SelectedItem,
                        x => x != null);

        CreateCommand = ReactiveCommand.CreateFromTask(ShowCreateDialogAsync);
        EditCommand = ReactiveCommand.CreateFromTask(ShowEditDialogAsync, removeOk);
        RemoveCommand = ReactiveCommand.CreateFromTask(RemoveTask, removeOk);

        RxApp.MainThreadScheduler.Schedule(LoadData);
    }

    public async Task ShowCreateDialogAsync()
    {
        var model = new CreateTaskItemViewModel(Users);
        var res = await CreateDialog.Handle(model);
        if (null != res && res is TaskItemViewModel task)
        {
            await _repository.AddAsync(task.Task);
            await _repository.SaveAsync();
            Tasks.Add(task);
        }
    }

    public async Task ShowEditDialogAsync()
    {
        var model = new EditTaskItemViewModel(SelectedItem!, Users);
        var res = await EditDialog.Handle(model);
        if (null != res && model.IsModified && res is TaskItemViewModel task)
        {
            var found = Tasks.FirstOrDefault(x => x.Task.Id == task.Task.Id);
            int index = Tasks.IndexOf(found!);
            Tasks[index] = task;
            await _repository.UpdateAsync(task.Task);
        }
    }

    public async Task RemoveTask()
    {
        if (null != SelectedItem)
        {
            _repository.Remove(SelectedItem.Task);
            await _repository.SaveAsync();
            Tasks.Remove(SelectedItem);
        }
    }

    private async void LoadData()
    {
        IsBusy = true;
        IsElementsVisible = false;

        // Only for showing progress-bar
        await Task.Delay(2000);

        await _repository.LoadTasksAsync();
        await _repository.LoadUsersAsync();
        var vm = (await _repository.GetTasksAsync())
                            .Select(x => new TaskItemViewModel((TaskItem)x));

        foreach (var t in vm)
        {
            Tasks.Add(t);
        }

        var users = await _repository.GetUsersAsync();
        foreach (var u in users)
        {
            Users.Add((User)u);
        }

        IsBusy = false;
        IsElementsVisible = true;
    }
}
