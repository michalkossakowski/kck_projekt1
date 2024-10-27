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
using System.Windows.Media.Animation;

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
            StartAnimation();
        }

        private void StartAnimation()
        {
            DoubleAnimation moveRight = new DoubleAnimation
            {
                To = 30,
                Duration = TimeSpan.FromMilliseconds(2000),
                AutoReverse = true, 
                RepeatBehavior = RepeatBehavior.Forever
            };
            iconNote.BeginAnimation(TranslateTransform.XProperty, moveRight);

            DoubleAnimation moveLeft = new DoubleAnimation
            {
                To = -30,
                Duration = TimeSpan.FromMilliseconds(2000),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            iconPencil.BeginAnimation(TranslateTransform.XProperty, moveLeft);


            DoubleAnimation moveDown = new DoubleAnimation
            {
                To = 5,
                Duration = TimeSpan.FromMilliseconds(500),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            iconPencil.BeginAnimation(TranslateTransform.YProperty, moveDown);

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
    }
}