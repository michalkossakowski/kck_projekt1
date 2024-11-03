using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using kck_api.Controller;
using MaterialDesignThemes.Wpf;

namespace kck_projekt2
{
    /// <summary>
    /// Logika interakcji dla klasy CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : UserControl
    {
        private MainWindow _mainWindow;
        private DateTime _selectedDay;
        NoteController _noteController;
        List<NoteModel> _currentMonthnotes;

        public CalendarPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _noteController  = NoteController.GetInstance();
            _currentMonthnotes = _noteController.GetNotesByUserIdAndMonth(_mainWindow.loggedUserId, DateTime.Now);
        }

        private void MyCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            MyCalendar.DisplayDateStart = firstDayOfMonth;
            MyCalendar.DisplayDateEnd = lastDayOfMonth;
            UpdateCalendarDays();
        }

        private void UpdateCalendarDays()
        {

            foreach (var day in FindChildren<CalendarDayButton>(MyCalendar))
            {
                day.Background = Brushes.Transparent; // Resetuj tło dla wszystkich dni

                if (((DateTime)day.DataContext).Date == _selectedDay.Date)
                {
                    day.BorderBrush = (SolidColorBrush)Application.Current.Resources["DarkColor"];
                }
                else if (_currentMonthnotes.Any(note => note.ModifiedDate.Date == ((DateTime)day.DataContext).Date))
                {
                    day.Background = (SolidColorBrush)Application.Current.Resources["MaterialDesign.Brush.Secondary"];
                }
            }
        }

        private void ShowNotesClick(object sender, RoutedEventArgs e)
        {
            if (_selectedDay == DateTime.MinValue)
            {
                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue("Select date in the calendar to show notes");
            }
            else
            {
                _mainWindow.contentControl.Content = new ExploreNotesByDayPage(_mainWindow, _selectedDay);
            }
        }

        private void MyCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedDay = MyCalendar.SelectedDate.Value;
            UpdateCalendarDays();
            this.Focus();
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
