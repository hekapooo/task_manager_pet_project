using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TaskManager.ViewModels;

namespace TaskManager.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

        this.WhenActivated(create =>
            create(ViewModel!.CreateDialog.RegisterHandler(DialogShowAsync)));
        this.WhenActivated(edit =>
            edit(ViewModel!.EditDialog.RegisterHandler(DialogShowAsync)));
    }

    private async Task DialogShowAsync(InteractionContext<ViewModelBase,
                                             ViewModelBase?> interaction)
    {
        if (interaction.Input is CreateTaskItemViewModel)
        {
            var dialog = new CreateTaskItemWindow();
            dialog.DataContext = interaction.Input;
            var result = await dialog.ShowDialog<ViewModelBase?>(this);
            interaction.SetOutput(result);
        }
        else if (interaction.Input is EditTaskItemViewModel)
        {
            var dialog = new EditTaskItemWindow();
            dialog.DataContext = interaction.Input;
            var result = await dialog.ShowDialog<ViewModelBase?>(this);
            interaction.SetOutput(result);
        }
    }
}
