using kck_projekt2.Commands;
using kck_projekt2.CustomElements;
using kck_projekt2.ViewModels;
using System.Windows;
using Forms = System.Windows.Forms;

namespace kck_projekt2
{
    public partial class App : Application
    {
        private readonly Forms.NotifyIcon _notifyIcon;
        private readonly NotifyViewModel _notifyViewModel;
        public App()
        {
            _notifyIcon = new Forms.NotifyIcon();
            _notifyViewModel = new NotifyViewModel(_notifyIcon);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();

            TrayImplementaion();

            MainWindow.DataContext = _notifyViewModel;
            MainWindow.StateChanged += MainWindow_StateChanged;

        }

        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            if (MainWindow.WindowState == WindowState.Minimized)
            {
                _notifyViewModel.NotifyCommand.Execute(new NotificationMessage("CNote#", "Aplikacja działa w tle.", Forms.ToolTipIcon.Info, 1000));
            }
        }

        private void TrayImplementaion() 
        {

            // Tray implementation

            _notifyIcon.Icon = new System.Drawing.Icon("Resources/icon.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "CNote#";

            _notifyIcon.Click += NotifyIcon_Click;
            _notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("About Program", null, OnAboutProgramClicked);
            _notifyIcon.ContextMenuStrip.Items.Add("Hide Tray", null, OnHideTrayClick);
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, OnExitClick);
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
