using kck_projekt2.Commands;
using System.Windows.Forms;
using System.Windows.Input;
using Forms = System.Windows.Forms;

namespace kck_projekt2.ViewModels
{
    public class NotifyViewModel : BaseViewModel
    {
        public ICommand NotifyCommand { get; }

        private bool _isDarkTheme;

        public NotifyViewModel(Forms.NotifyIcon notifyIcon)
        {
            NotifyCommand = new NotifyCommand(notifyIcon);
        }
        public NotificationMessage ConsoleModeNotificationMessage
        {
            get
            {
                return new NotificationMessage(
                    "Aplikacja w trybie konsolowym",
                    "Aplikacja została uruchomiona w trybie konsolowym.",
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
                string title = _isDarkTheme ? "Ciemny motyw" : "Jasny motyw";
                string content = _isDarkTheme ? "Aplikacja została przełączona na ciemny motyw." : "Aplikacja została przełączona na jasny motyw.";
                return new NotificationMessage(title, content, ToolTipIcon.Info, 3000);
            }
        }
    }
}
