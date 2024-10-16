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
            string nickValue = nick.Text;
            string passwordValue = password.Password;

            if (nickValue.Length == 0 || passwordValue.Length == 0)
            {
                errorMessage.Content = "Nick and password cannot be null !";
                errorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                var userController = UserController.GetInstance();
                var user = new UserModel(nickValue, passwordValue);
                user = await Task.Run(() => userController.GetUser(user));
                if (user == null)
                {
                    errorMessage.Content = "Wrong nick or password !";
                    errorMessage.Visibility = Visibility.Visible;

                }
                else
                {
                    _mainWindow.loggedUserId = user.Id;
                    _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
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
