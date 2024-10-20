using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class CalendarPage : UserControl
    {
        private MainWindow _mainWindow;
        public CalendarPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            myCalendar.SelectedDatesChanged += MyCalendar_SelectedDatesChanged;
        }
        private void MyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = myCalendar.SelectedDate;

            if (selectedDate.HasValue)
            {
                _mainWindow.contentControl.Content = new ExploreNotesByDayPage(_mainWindow, selectedDate.Value);
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }

    }
}
