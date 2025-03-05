using kck_projekt2.Commands;
using kck_projekt2.CustomElements;
using kck_projekt2.ViewModels;
using System.Windows;
using System.Windows.Controls.Primitives;
using Forms = System.Windows.Forms;

namespace kck_projekt2
{
    public partial class App : Application
    {
        public static event Action LanguageChanged;

        private readonly Forms.NotifyIcon _notifyIcon;
        public App()
        {
            _notifyIcon = new Forms.NotifyIcon();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();

            TrayImplementaion();
            MainViewModel mainViewModel = new MainViewModel(Current.MainWindow as MainWindow, _notifyIcon);
            Current.MainWindow.DataContext = mainViewModel;
            MainWindow.StateChanged += mainViewModel.MainWindow_StateChanged;
        }

        private void TrayImplementaion() 
        {

            // Tray implementation

            _notifyIcon.Icon = new System.Drawing.Icon("Resources/icon.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "CNote#";

            _notifyIcon.Click += NotifyIcon_Click;
            UpdateTrayMenu();
            LanguageChanged += UpdateTrayMenu;
        }
        private void UpdateTrayMenu()
        {
            if (_notifyIcon.ContextMenuStrip != null)
            {
                _notifyIcon.ContextMenuStrip.Dispose();
            }

            _notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add((string)Application.Current.Resources["TrayAboutProgramStr"], null, OnAboutProgramClicked);
            _notifyIcon.ContextMenuStrip.Items.Add((string)Application.Current.Resources["TrayHideTrayStr"], null, OnHideTrayClick);
            _notifyIcon.ContextMenuStrip.Items.Add((string)Application.Current.Resources["TrayExitStr"], null, OnExitClick);
            _notifyIcon.ContextMenuStrip.Items.Add((string)Application.Current.Resources["TrayChangeLanguagetoStr"], null, SwitchLanguage);
        }


        private void SwitchLanguage(object? sender, EventArgs e)
        {
            var mainWindow = Current.MainWindow as MainWindow;
            if(mainWindow.IsPolish is ToggleButton toggleButton)
                toggleButton.IsChecked = !toggleButton.IsChecked;

            mainWindow.SwitchLang(sender, e as RoutedEventArgs);
            LanguageChanged?.Invoke();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();
            base.OnExit(e);
        }


        private void OnAboutProgramClicked(object? sender, EventArgs e)
        {
            CustomMessageBox msgBox = new CustomMessageBox();
            msgBox.ShowDialog();
        }
        private void OnExitClick(object? sender, EventArgs e)
        {
            _notifyIcon.Dispose();
            Shutdown();
        }
        private void OnHideTrayClick(object? sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
        }
        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        }
    }
}
