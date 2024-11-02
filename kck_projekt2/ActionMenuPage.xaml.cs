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
            _mainWindow.contentControl.Content = new ExploreNotesPage(_mainWindow);
        }
        private void OpenCalendarPage(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new CalendarPage(_mainWindow);
        }
        private void OpenSearchPage(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new SearchPage(_mainWindow,null);
        }
        private void OpenFindByDatePage(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new FindByDatePage(_mainWindow,null);
        }
        private void OpenFindByCategoryPage(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new FindByCategoryPage(_mainWindow,null);
        }
        private void OpenExoplreNotesByMonthPage(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ExploreNotesByMonthPage(_mainWindow, null);
        }
    }
}
