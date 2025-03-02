using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using kck_api.Controller;
using MaterialDesignThemes.Wpf;

namespace kck_projekt2
{
    public partial class RegisterPage : UserControl
    {
        private MainWindow _mainWindow;

        public RegisterPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private async void RegisterClick(object sender, RoutedEventArgs e)
        {
            Loading.Visibility = Visibility.Visible;
            LoginButton.IsEnabled = false;
            BackButton.IsEnabled = false;
            nick.IsEnabled = false;
            password.IsEnabled = false;
            confirmPassword.IsEnabled = false;
            string nickValue = nick.Text;
            string passwordValue = password.Password;
            string confirmPasswordValue = confirmPassword.Password;

            if (nickValue.Length == 0 || passwordValue.Length == 0 || confirmPasswordValue.Length ==  0)
            {
                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["NickAndPasswordEmptyStr"]);
                if (nickValue.Length == 0)
                    nick.Foreground = new SolidColorBrush(Colors.Red);
                if (passwordValue.Length == 0)
                    password.Foreground = new SolidColorBrush(Colors.Red);
                if (confirmPasswordValue.Length == 0)
                    confirmPassword.Foreground = new SolidColorBrush(Colors.Red);

            }
            else if (passwordValue != confirmPasswordValue)
            {
                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["PasswordsDontMatchStr"]);
                password.Foreground = new SolidColorBrush(Colors.Red);
                confirmPassword.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                var userController = UserController.GetInstance();
                var user = new UserModel(nickValue, passwordValue);
                bool isCreated = false;
                try
                {
                    isCreated = await Task.Run(async () =>
                    {
                        await Task.Delay(1000);
                        return await userController.AddUserAsync(user);

                    });
                    if (isCreated)
                    {
                        _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                        _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
                        _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["RegisterSuccesStr"]);
                        _mainWindow.ReturnToMainMenu();
                    }
                    else
                    {
                        _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                        _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                        nick.Foreground = new SolidColorBrush(Colors.Red);
                        _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["NickOccupiedStr"]);
                    }
                }
                catch(InvalidOperationException ex)
                {
                    _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                    _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                    _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["DBErrorStr"]);
                }    
            }
            LoginButton.IsEnabled = true;
            BackButton.IsEnabled = true;
            nick.IsEnabled = true;
            password.IsEnabled = true;
            confirmPassword.IsEnabled = true;
            Loading.Visibility = Visibility.Hidden;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.ReturnToMainMenu();
        }

        private void Nick_TextChanged(object sender, TextChangedEventArgs e)
        {
            nick.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
            if (password.Password.Length != 0)
                password.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
        }

        private void Password_TextChanged(object sender, RoutedEventArgs e)
        {
            password.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
            if (nick.Text.Length != 0)
                nick.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
            confirmPassword.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
        }

        private void ConfirmPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            confirmPassword.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
            if (nick.Text.Length != 0)
                nick.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
        }
    }
}
