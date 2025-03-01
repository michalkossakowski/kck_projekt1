using System.Windows.Forms;
using System.Windows.Input;

namespace kck_projekt2.Commands
{
    public class NotifyCommand : ICommand
    {
        private readonly NotifyIcon _notifyIcon;

        public event EventHandler? CanExecuteChanged;

        public NotifyCommand(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is NotificationMessage message)
            {
                _notifyIcon.ShowBalloonTip(
                    message.Duration,
                    message.Title,
                    message.Content,
                    message.IconType ?? ToolTipIcon.None
                );
            }
            else
            {
                _notifyIcon.ShowBalloonTip(2000, "CNote#", "CNote# is running in the background.", ToolTipIcon.Info);
            }
        }
    }

    public class NotificationMessage
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public ToolTipIcon? IconType { get; set; } = null;
        public int Duration { get; set; } = 3000; // Domyślnie 3 sekundy

        public NotificationMessage(string title, string content, ToolTipIcon iconType, int duration = 3000)
        {
            Title = title;
            Content = content;
            IconType = iconType;
            Duration = duration;
        }
    }
}
