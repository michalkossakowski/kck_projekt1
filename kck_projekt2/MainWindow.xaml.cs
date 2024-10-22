using System.Diagnostics;
using System.IO;
using System.Text;
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
using kck_api.Database;

namespace kck_projekt2
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int loggedUserId;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenLoginPage(object sender, RoutedEventArgs e)
        {
            WelcomePanel.Visibility = Visibility.Collapsed;
            contentControl.Content = new LoginPage(this);
        }

        private void OpenRegisterPage(object sender, RoutedEventArgs e)
        {
            WelcomePanel.Visibility = Visibility.Collapsed;
            contentControl.Content = new RegisterPage(this);
        }

        private void OpenActionMenu(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new ActionMenuPage(this);
        }

        private void SwitchToConsoleMode(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "kck_projekt1.exe");
            process.Start();
            Environment.Exit(0);
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            LogoutMenuItem.Visibility = Visibility.Collapsed;
            ActionMenuMenuItem.Visibility = Visibility.Collapsed;
            ReturnToMainMenu();
        }


        public void ReturnToMainMenu()
        {
            WelcomePanel.Visibility = Visibility.Visible;
            contentControl.Content = null;
            loggedUserId = -1;
        }

        private void Image_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {

        }
    }
}