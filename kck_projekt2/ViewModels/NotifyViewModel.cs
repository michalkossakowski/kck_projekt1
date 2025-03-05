using kck_projekt2.Commands;
using System.Windows.Forms;
using System.Windows.Input;
using Forms = System.Windows.Forms;
using W = System.Windows;
namespace kck_projekt2.ViewModels
{
    public class NotifyViewModel : BaseViewModel
    {
        public ICommand NotifyCommand { get; }
        public ICommand UpdateTrayCommand { get; }

        private bool _isDarkTheme;
        private readonly Forms.NotifyIcon _notifyIcon;

        public NotifyViewModel(Forms.NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
            NotifyCommand = new NotifyCommand(notifyIcon);
        }
        public NotificationMessage ConsoleModeNotificationMessage
        {
            get
            {
                return new NotificationMessage(
                    (string)W.Application.Current.Resources["NotifConsoleTitleStr"],
                    (string)W.Application.Current.Resources["NotifConsoleContentStr"],
                    ToolTipIcon.Info,
                    3000
                );
            }
        }

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (_isDarkTheme != value)
                {
                    _isDarkTheme = value;
                    OnPropertyChanged();
                    // W momencie zmiany motywu, tworzymy powiadomienie o zmianie motywu
                    OnPropertyChanged(nameof(SwitchThemeModeNotificationMessage));
                }
            }
        }

        public NotificationMessage SwitchThemeModeNotificationMessage
        {
            get
            {
                string title = _isDarkTheme ? 
                    (string)W.Application.Current.Resources["DarkModeStr"] 
                    : (string)W.Application.Current.Resources["LightModeStr"];

                string content = _isDarkTheme 
                    ? (string)W.Application.Current.Resources["DarkModeMessageContentStr"] 
                    : (string)W.Application.Current.Resources["LightModeMessageContentStr"];
                return new NotificationMessage(title, content, ToolTipIcon.Info, 3000);
            }
        }
    }
}
