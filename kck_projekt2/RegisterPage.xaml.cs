using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using kck_api.Controller;

namespace kck_projekt2
{
    /// <summary>
    /// Logika interakcji dla klasy LoginPage.xaml
    /// </summary>
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
            string nickValue = nick.Text;
            string passwordValue = password.Password;
            string confirmValue = confirm.Password;

            if (nickValue.Length == 0 || passwordValue.Length == 0 || confirmValue.Length ==  0)
            {
                errorMessage.Content = "Nick, password and confirmation cannot be null !";
                errorMessage.Visibility = Visibility.Visible;
            }
            else if (passwordValue != confirmValue)
            {
                errorMessage.Content = ("Passwords don't match !");
            }
            else
            {
                var userController = UserController.GetInstance();
                var user = new UserModel(nickValue, passwordValue);
                if (await Task.Run(() => userController.AddUser(user)))
                {
                    errorMessage.Content = ("New user added you can login now !");
                    errorMessage.Foreground = new SolidColorBrush(Colors.Green);
                    Loading.Visibility = Visibility.Hidden;
                    nick.IsEnabled = false;
                    password.IsEnabled = false;
                    confirm.IsEnabled = false;
                    return;
                }
                else
                {
                    errorMessage.Content = ("This nick is occupied !");
                }
            }
            LoginButton.IsEnabled = true;
            Loading.Visibility = Visibility.Hidden;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.ReturnToMainMenu();
        }

    }
}
