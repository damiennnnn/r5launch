using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace CustomTitleBarTemplate.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Window hostWindow;
        public ReactiveCommand<Unit, Unit> QuitProgramCommand { get; }
        public ReactiveCommand<Unit, Unit> SwitchThemeCommand { get; }
        public MainWindowViewModel (Window _hostWindow)
        {
            hostWindow = _hostWindow;
            
            QuitProgramCommand = ReactiveCommand.Create(() => { hostWindow.Close(); });
            SwitchThemeCommand = ReactiveCommand.Create(() => { (hostWindow as Views.MainWindow).SwitchTheme(); });
        }

    }
}
