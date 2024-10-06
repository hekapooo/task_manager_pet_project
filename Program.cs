using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace TaskManager;

// Решил сделать полностью desktop приложение, поэтому "много" окон:)
// Старался реализовать все через ReactiveUI (кнопки, открытие окон), поэтому 
// получилось почти полностью избавиться от code-behinde.
// В базе 2 таблицы, Tasks и Users.
// Всю работу с базой сделал асинхронной.
// Не нашел еще чего-то, чем еще можно было бы воспользоваться, чтобы сделать код еще
// более "слабосвязанным". IoC не стал тащить ради 2 строчек кода.
// Данный проект на Avalonia также заработал на win, у коллеги получилось запустить на android,
// считаю, что данный UI framework достоин внимания :)
// Также поработал с деревьями выражений, удобная штука :)

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
