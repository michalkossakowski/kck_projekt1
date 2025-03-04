using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using kck_api.Controller;
using MaterialDesignThemes.Wpf;

namespace kck_projekt2
{

    public partial class LoginPage : UserControl
    {
        private MainWindow _mainWindow;

        public LoginPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            Loading.Visibility = Visibility.Visible;
            LoginButton.IsEnabled = false;
            BackButton.IsEnabled = false;
            password.IsEnabled = false;
            nick.IsEnabled = false;
            string nickValue = nick.Text;
            string passwordValue = password.Password;

            if (nickValue.Length == 0 || passwordValue.Length == 0)
            {
                if(nickValue.Length == 0)
                    nick.Foreground = new SolidColorBrush(Colors.Red);
                if(passwordValue.Length == 0)
                    password.Foreground = new SolidColorBrush(Colors.Red);

                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["NickAndPasswordEmptyStr"]);
            }
            else
            {
                var userController = UserController.GetInstance();
                var user = new UserModel(nickValue, passwordValue);
                try
                {
                    user = await Task.Run(async () =>
                    {
                        await Task.Delay(1000);
                        return await userController.GetUserAsync(user);
                    });
                    if (user == null)
                    {
                        _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                        _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                        nick.Foreground = new SolidColorBrush(Colors.Red);
                        password.Foreground = new SolidColorBrush(Colors.Red);
                        _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["WrongLoginDataStr"]);
                    }
                    else
                    {
                        _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                        _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                        _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["LoginSuccesStr"]);

                        _mainWindow.LogoutMenuItem.Visibility = Visibility.Visible;
                        _mainWindow.ActionMenuMenuItem.Visibility = Visibility.Visible;
                        _mainWindow.AIMenuItem.Visibility = Visibility.Visible;
                        _mainWindow.BackToMenu.Visibility = Visibility.Visible;
                        _mainWindow.loggedUserId = user.Id;
                        _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
                    }
                }
                catch(InvalidOperationException ex)
                {
                    _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                    _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                    _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["DBErrorStr"]);
                }

            }
            BackButton.IsEnabled = true;
            LoginButton.IsEnabled = true;
            password.IsEnabled = true;
            nick.IsEnabled = true;
            Loading.Visibility = Visibility.Hidden;
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
            if(nick.Text.Length != 0)
                nick.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.ReturnToMainMenu();
        }
    }
}
