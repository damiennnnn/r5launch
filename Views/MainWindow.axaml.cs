using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using CustomTitleBarTemplate.ViewModels;
using System.Timers;

namespace CustomTitleBarTemplate.Views
{
    public class MainWindow : Window
    {
        private ToggleButton darkThemeToggleButton;
        private ToggleButton defaultStyleToggleButton;

        private ProgressBar progressBar;
        private TextBlock mainPageButton;
        private TextBlock modPageButton;
        private TextBlock settingsPageButton;

        private Border MainPage;
        private Border ModPage;
        private Border SettingsPage;

        public double progress = 0;
        private int currentPage = 0;
        // 0 = main, 1 = mod

        private bool isDefaultStyle = false;
        private bool isDarkTheme = false;
        public MainWindow()
        {
            this.InitializeComponent();

            #if DEBUG
            this.AttachDevTools();
            #endif

            this.DataContext = new MainWindowViewModel(this);

            progressBar = this.FindControl<ProgressBar>("progBar");
            mainPageButton = this.FindControl<TextBlock>("mainButton");
            modPageButton = this.FindControl<TextBlock>("modButton");
            settingsPageButton = this.FindControl<TextBlock>("settingsButton");

            MainPage = this.FindControl<Border>("MainPage");
            ModPage = this.FindControl<Border>("ModPage");
            SettingsPage = this.FindControl<Border>("SettingsPage");

            mainPageButton.PointerPressed += MainPageClick;
            modPageButton.PointerPressed += ModPageClick;
            settingsPageButton.PointerPressed += SettingsPageClick;

            this.CanResize = false;
            this.Height = 500;
            this.Width = 800;
            this.Padding = new Thickness(
                            this.OffScreenMargin.Left,
                            this.OffScreenMargin.Top,
                            this.OffScreenMargin.Right,
                            this.OffScreenMargin.Bottom);

            Application.Current.Styles[1] = App.FluentLight;
            SwitchTheme();
            SetVisibility(MainPage, mainPageButton, true);
            SetVisibility(SettingsPage, settingsPageButton, false);
            SetVisibility(ModPage, modPageButton, false);

            Timer timer = new Timer();
            timer.Interval = 20;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

        }

        private void SetVisibility(Border border, TextBlock button, bool visible)
        {
            border.IsEnabled = visible;
            border.IsVisible = visible;
            button.Background = new SolidColorBrush(Color.FromRgb(38, 38, 38), visible ? 1 : 0);
        }

        private void SettingsPageClick(object sender, PointerPressedEventArgs e)
        {
            SetVisibility(MainPage, mainPageButton, false);
            SetVisibility(SettingsPage, settingsPageButton, true);
            SetVisibility(ModPage, modPageButton, false);
        }


        private void ModPageClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SetVisibility(MainPage, mainPageButton, false);
            SetVisibility(SettingsPage, settingsPageButton, false);
            SetVisibility(ModPage, modPageButton, true);
        }

        private void MainPageClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SetVisibility(MainPage, mainPageButton, true);
            SetVisibility(SettingsPage, settingsPageButton, false);
            SetVisibility(ModPage, modPageButton, false);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() => 
            progressBar.Value += 1);
        }

        public void SwitchTheme()
        {
            if (isDarkTheme)
                SetLightTheme(null, null);
            else
                SetDarkTheme(null, null); // sender and eventargs have no use here
        }

        private void SetLightTheme(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Cursor = new Cursor(StandardCursorType.Wait);
            Application.Current.Styles[1] = isDefaultStyle ? App.DefaultLight : App.FluentLight;
            Application.Current.Resources["MacOsTitleBarBackground"] = new SolidColorBrush { Color = new Color(255, 222, 225, 230) };
            Application.Current.Resources["MacOsWindowTitleColor"] = new SolidColorBrush { Color = new Color(255, 77, 77, 77) };
            Cursor = new Cursor(StandardCursorType.Arrow);
            isDarkTheme = false;
        }

        private void SetDarkTheme(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Cursor = new Cursor(StandardCursorType.Wait);
            Application.Current.Styles[1] = isDefaultStyle ? App.DefaultDark : App.FluentDark;
            Application.Current.Resources["MacOsTitleBarBackground"] = new SolidColorBrush { Color = new Color(255, 62, 62, 64) };
            Application.Current.Resources["MacOsWindowTitleColor"] = new SolidColorBrush { Color = new Color(255, 153, 158, 161) };
            Cursor = new Cursor(StandardCursorType.Arrow);
            isDarkTheme = true;

        }

        private void UseNativeTitleBar()
        {
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.SystemChrome;
            ExtendClientAreaTitleBarHeightHint = -1;
            ExtendClientAreaToDecorationsHint = false;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
