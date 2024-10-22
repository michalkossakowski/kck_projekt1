using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using kck_api.Controller;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            MyCalendar.SelectedDatesChanged += MyCalendar_SelectedDatesChanged;
        }

        private void MyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ExploreNotesByDayPage(_mainWindow, MyCalendar.SelectedDate.Value);
        }

        private void MyCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            var noteController = NoteController.GetInstance();
            var currentMonthNotes = noteController.GetCurrentMonthNotesByUserId(_mainWindow.loggedUserId, DateTime.Now);

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            MyCalendar.DisplayDateStart = firstDayOfMonth;
            MyCalendar.DisplayDateEnd = lastDayOfMonth;

            foreach (var note in currentMonthNotes)
            {
                foreach (var day in FindChildren<CalendarDayButton>(MyCalendar))
                    if (((DateTime)day.DataContext).Date == note.ModifiedDate.Date)
                        day.Background = Brushes.LightSkyBlue;
            }
        }

        public static List<CalendarDayButton> FindChildren<CalendarDayButton>(DependencyObject parent)
        {
            var days = new List<CalendarDayButton>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is CalendarDayButton typedChild)
                    days.Add(typedChild);

                days.AddRange(FindChildren<CalendarDayButton>(child));
            }

            return days;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }

    }
}
