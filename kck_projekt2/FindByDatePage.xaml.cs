using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using kck_api.Controller;

namespace kck_projekt2
{
    public partial class FindByDatePage : UserControl
    {
        private MainWindow _mainWindow;
        public ObservableCollection<NoteModel> Notes { get; set; }
        private NoteController _noteController;
        public DateTime _searchingDate;
        public FindByDatePage(MainWindow mainWindow, DateTime? searchingDate)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _noteController = NoteController.GetInstance();
            if (searchingDate != null)
            {
                DatePicker.SelectedDate = searchingDate;
                _searchingDate = (DateTime)searchingDate;
                Search();
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            _searchingDate = (DateTime)DatePicker.SelectedDate;
            Search();
        }

        private void Search()
        {
            var notes = _noteController.GetNotesByUserIdAndDay(_mainWindow.loggedUserId, _searchingDate);
            if (notes.Count == 0)
            {
                BottomTip.Visibility = Visibility.Hidden;
                Information.Visibility = Visibility.Visible;
                Information.Text = $"You dont have any notes from: \"{_searchingDate:dd.MM.yyyy}\"";
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
