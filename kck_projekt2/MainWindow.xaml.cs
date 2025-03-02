using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using kck_projekt2.Commands;
using kck_projekt2.ViewModels;
using MaterialDesignThemes.Wpf;

namespace kck_projekt2
{
    public partial class MainWindow : Window
    {
        public int loggedUserId;
        public MainWindow()
        {
            InitializeComponent();
            StartAnimation();
        }

        public async void SwitchThemeClick(object sender, RoutedEventArgs e) 
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(IsDarkTheme.IsChecked == true ? BaseTheme.Dark : BaseTheme.Light);
            if (IsDarkTheme.IsChecked == true)
            {
                Application.Current.Resources["TextBlockColor"] = new SolidColorBrush(Colors.White);
                Application.Current.Resources["TextBoxColor"] = new SolidColorBrush(Colors.White);
                Application.Current.Resources["HoverColor"] = new SolidColorBrush(Color.FromRgb(79, 79, 79));
                Application.Current.Resources["NoHoverColor"] = new SolidColorBrush(Color.FromRgb(39, 39, 39));
                theme.SetBaseTheme(BaseTheme.Dark);
                theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#aa00ff"));
                theme.SetSecondaryColor((Color)ColorConverter.ConvertFromString("#673ab7"));
            }
            else
            {
                Application.Current.Resources["TextBlockColor"] = (SolidColorBrush)Application.Current.Resources["DarkColor"];
                Application.Current.Resources["TextBoxColor"] = new SolidColorBrush(Colors.Black);
                Application.Current.Resources["HoverColor"] = new SolidColorBrush(Color.FromRgb(220, 204, 248));
                Application.Current.Resources["NoHoverColor"] = new SolidColorBrush(Color.FromRgb(237, 230, 250));
                theme.SetBaseTheme(BaseTheme.Light);
                theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#673ab7"));
                theme.SetSecondaryColor((Color)ColorConverter.ConvertFromString("#aa00ff"));
            }
            paletteHelper.SetTheme(theme);
        }

        private void StartAnimation()
        {
            DoubleAnimation moveRight = new DoubleAnimation
            {
                To = 25,
                Duration = TimeSpan.FromMilliseconds(2500),
                AutoReverse = true, 
                RepeatBehavior = RepeatBehavior.Forever
            };
            iconNote.BeginAnimation(TranslateTransform.XProperty, moveRight);


            DoubleAnimation moveLeft = new DoubleAnimation
            {
                To = -20,
                Duration = TimeSpan.FromMilliseconds(2500),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            iconPencil.BeginAnimation(TranslateTransform.XProperty, moveLeft);

            DoubleAnimation moveDown = new DoubleAnimation
            {
                To = 5,
                Duration = TimeSpan.FromMilliseconds(200),
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
            try
            { 
                Process process = new Process();
                process.StartInfo.FileName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "CNote#Console.exe");
                process.Start();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Console Mode is not currently available");
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            LogoutMenuItem.Visibility = Visibility.Collapsed;
            ActionMenuMenuItem.Visibility = Visibility.Collapsed;
            AIMenuItem.Visibility = Visibility.Collapsed;
            ReturnToMainMenu();
        }

        public void ReturnToMainMenu()
        {
            WelcomePanel.Visibility = Visibility.Visible;
            contentControl.Content = null;
            loggedUserId = -1;
        }
        private void OpenChatButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChatWindow chatWindow = new ChatWindow();
                chatWindow.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}