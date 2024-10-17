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

namespace kck_projekt2
{
    /// <summary>
    /// Logika interakcji dla klasy ActionMenuPage.xaml
    /// </summary>
    public partial class ActionMenuPage : UserControl
    {
        MainWindow _mainWindow;

        public ActionMenuPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void OpenAddNotePage(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new AddNotePage(_mainWindow);
        }
        private void OpenLatestNotesPage(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new LatestNotesPage(_mainWindow);
        }
        private void OpenExploreNotesPage(object sender, RoutedEventArgs e)
        {

        }
        private void OpenCalendarPage(object sender, RoutedEventArgs e)
        {

        }
        private void OpenSearchPage(object sender, RoutedEventArgs e)
        {

        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.ReturnToMainMenu();
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
