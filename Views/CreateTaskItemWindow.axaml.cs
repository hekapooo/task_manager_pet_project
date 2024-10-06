using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TaskManager.ViewModels;

namespace TaskManager.Views;

public partial class CreateTaskItemWindow : ReactiveWindow<CreateTaskItemViewModel>
{
    public CreateTaskItemWindow()
    {
        InitializeComponent();
        this.WhenActivated(action => action(ViewModel!.SaveCommand.Subscribe(Close)));
    }
}