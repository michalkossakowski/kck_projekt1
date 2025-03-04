using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace kck_projekt2
{
    public partial class CalendarPage : UserControl
    {
        private readonly CalendarViewModel _viewModel;

        public CalendarPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _viewModel = new CalendarViewModel(mainWindow);
            DataContext = _viewModel;
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void MyCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            MyCalendar.DisplayDateStart = firstDayOfMonth;
            MyCalendar.DisplayDateEnd = lastDayOfMonth;
            _viewModel.SelectedDay = DateTime.Now;
            UpdateCalendarDays();
        }

        private void MyCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyCalendar.SelectedDate.HasValue)
            {
                _viewModel.SelectedDay = MyCalendar.SelectedDate.Value;
            }
            this.Focus();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedDay) || e.PropertyName == nameof(_viewModel.CurrentMonthNotes))
            {
                UpdateCalendarDays();
            }
        }

        private void UpdateCalendarDays()
        {
            foreach (var day in FindChildren<CalendarDayButton>(MyCalendar))
            {
                day.Background = Brushes.Transparent;

                if (((DateTime)day.DataContext).Date == _viewModel.SelectedDay.Date)
                {
                    day.BorderBrush = (SolidColorBrush)Application.Current.Resources["DarkColor"];
                }
                else if (_viewModel.CurrentMonthNotes.Any(note => note.ModifiedDate.Date == ((DateTime)day.DataContext).Date))
                {
                    day.Background = (SolidColorBrush)Application.Current.Resources["MaterialDesign.Brush.Secondary"];
                }
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
    }
}