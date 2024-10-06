using System.Reactive;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TaskManager.Services;
using TaskManager.ViewModels;
using TaskManager.Views;

namespace TaskManager;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            UnitOfWork unit = new UnitOfWork();
            IRepository rep = new EFRepository(unit);
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(rep),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}