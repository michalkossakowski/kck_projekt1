using kck_projekt2.Commands;
using System.Windows;
using Forms = System.Windows.Forms;
namespace kck_projekt2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public NotifyViewModel NotifyVM { get; }
        public TopNavMenuViewModel MenuVM { get; }

        private readonly MainWindow _mainWindow;
        public MainViewModel(MainWindow mainWindow, Forms.NotifyIcon notifyIcon)
        {
            _mainWindow = mainWindow;
            NotifyVM = new NotifyViewModel(notifyIcon);
            MenuVM = new TopNavMenuViewModel(mainWindow);
        }

        public void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            if (_mainWindow.WindowState == WindowState.Minimized)
            {
                NotifyVM.NotifyCommand.Execute(new NotificationMessage("CNote#", (string)Application.Current.Resources["NotifyRunningInBackStr"], Forms.ToolTipIcon.Info, 1000));
            }
        }
        public bool IsDarkTheme
        {
            get => NotifyVM.IsDarkTheme;
            set => NotifyVM.IsDarkTheme = value;
        }
    }

}
