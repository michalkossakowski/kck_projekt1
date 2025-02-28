using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using kck_api.Controller;

namespace kck_projekt2
{
    public partial class ExploreNotesByMonthPage : UserControl, INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        public ObservableCollection<NoteModel> Notes { get; set; }
        public ObservableCollection<string> YearAndMonth { get; set; }
        private NoteController _noteController;
        public DateTime _searchingDate;
        private string _selectedYearMonth;
        public event PropertyChangedEventHandler PropertyChanged;

        public ExploreNotesByMonthPage(MainWindow mainWindow, DateTime? searchingDate)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _noteController = NoteController.GetInstance();

            if (searchingDate != null)
            {
                _searchingDate = (DateTime)searchingDate;
                SelectedYearMonth = $"{_searchingDate.Month}.{_searchingDate.Year}";
                Search();
            }
            var allNotes = _noteController.GetNotesByUserId(_mainWindow.loggedUserId);
            if(allNotes.Count() == 0)
            {
                Information.Visibility = Visibility.Visible;
                Information.Text = "You dont have any notes";
                Information.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                SelectedMonth.IsEnabled = false;
                SearchButton.IsEnabled = false;
            }
            else
            {
                var yearAndMonth = new HashSet<(int, int)>();
                foreach (var note in allNotes)
                {
                    yearAndMonth.Add((note.ModifiedDate.Year, note.ModifiedDate.Month));
                }
                YearAndMonth = new ObservableCollection<string>(
                    yearAndMonth.Select(ym => $"{ym.Item2}.{ym.Item1}")
                );
                DataContext = this;
            }
        }

        public string SelectedYearMonth
        {
            get => _selectedYearMonth;
            set
            {
                if (_selectedYearMonth != value)
                {
                    _selectedYearMonth = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedYearMonth)));
                }
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            DataContext = null;

            if (!string.IsNullOrEmpty(SelectedYearMonth))
            {
                var month = int.Parse(SelectedYearMonth.Split('.')[0]);
                var year = int.Parse(SelectedYearMonth.Split('.')[1]);
                _searchingDate = new DateTime(year, month, 1);
                Search();
            }
            else
            {
                Information.Visibility = Visibility.Visible;
                Information.Text = "Searching month cannot be null";
                Information.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

        private void Search()
        {
            var notes = _noteController.GetNotesByUserIdAndMonth(_mainWindow.loggedUserId, _searchingDate);
            if (notes.Count == 0)
            {
                Information.Visibility = Visibility.Visible;
                Information.Text = "There is no notes that match chosen month";
                Information.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else
            {
                BottomTip.Visibility = Visibility.Visible;
                Information.Visibility = Visibility.Collapsed;
                Notes = new ObservableCollection<NoteModel>(notes);
                DataContext = this;
            }
        }

        private void OpenEditPage(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is NoteModel note)
            {
                _mainWindow.contentControl.Content = new EditNotePage(_mainWindow, note.Id, this);
            }
        }
    }
}
