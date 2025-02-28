using System.Windows;
using System.Windows.Input;

namespace kck_projekt2
{
    public class YesNoDialogViewModel : BaseViewModel
    {
        private readonly Window _window;
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        public ICommand YesCommand { get; }
        public ICommand NoCommand { get; }

        public YesNoDialogViewModel(Window window, string message)
        {
            _window = window;
            Message = message;

            YesCommand = new RelayCommand(Yes);
            NoCommand = new RelayCommand(No);
        }

        private void Yes()
        {
            _window.DialogResult = true;
            _window.Close();
        }

        private void No()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}