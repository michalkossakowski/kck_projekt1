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
    public class SampleItem()
    {

    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenLoginPage(object sender, RoutedEventArgs e)
        {
            MainMenuGrid.Visibility = Visibility.Collapsed;
            contentControl.Content = new LoginPage(this);
        }

        private void OpenRegisterPage(object sender, RoutedEventArgs e)
        {
            MainMenuGrid.Visibility = Visibility.Collapsed;
            contentControl.Content = new RegisterPage(this);
        }

        private void SwitchToConsoleMode(object sender, RoutedEventArgs e)
        {
            string exePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "kck_projekt1.exe");
            Process process = new Process();
            process.StartInfo.FileName = exePath;
            process.Start();
            Environment.Exit(0);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public void ReturnToMainMenu()
        {
            MainMenuGrid.Visibility = Visibility.Visible;
            contentControl.Content = null;
        }
    }
}