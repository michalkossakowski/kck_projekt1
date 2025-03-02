using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using kck_api.Controller;

namespace kck_projekt2
{
    public partial class SearchPage : UserControl
    {
        private MainWindow _mainWindow;
        public ObservableCollection<NoteModel> Notes { get; set; }
        private NoteController _noteController;
        public string _search;
        public SearchPage(MainWindow mainWindow, string? search)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _noteController = NoteController.GetInstance();
            if (search != null)
            {
                SearchInput.Text = search;
                _search = search;
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
            _search = SearchInput.Text;
            if (_search.Length == 0)
            {
                Information.Visibility = Visibility.Visible;
                Information.Text = (string)Application.Current.Resources["EmptySearchStr"];
                Information.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                BottomTip.Visibility = Visibility.Hidden;
            }
            else
            {
                Search();
            }
        }

        private async void Search()
        {
            var notes = await _noteController.GetNotesByUserIdAndTitleAsync(_mainWindow.loggedUserId,_search);
            if (notes.Count == 0)
            {
                BottomTip.Visibility = Visibility.Hidden;
                Information.Visibility = Visibility.Visible;
                Information.Text = (string)Application.Current.Resources["NoSearchResultStr"] + $" \"{_search}\"";
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
