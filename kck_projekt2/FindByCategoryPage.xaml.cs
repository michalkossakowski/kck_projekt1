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
    public partial class FindByCategoryPage : UserControl
    {
        private MainWindow _mainWindow;
        public ObservableCollection<NoteModel> Notes { get; set; }
        private NoteController _noteController;
        public string _searchingCategory;
        public FindByCategoryPage(MainWindow mainWindow, string? searchingCategory)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _noteController = NoteController.GetInstance();
            if (searchingCategory != null)
            {
                CategoryInput.Text = searchingCategory;
                _searchingCategory = searchingCategory;
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
            _searchingCategory = CategoryInput.Text;
            if (_searchingCategory.Length == 0)
            {
                Information.Visibility = Visibility.Visible;
                Information.Text = "Category search input cannot be empty";
                Information.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                BottomTip.Visibility = Visibility.Hidden;
            }
            else
            {
                Search();
            }
        }

        private void Search()
        {
            var notes = _noteController.GetNotesByUserIdAndCategory(_mainWindow.loggedUserId, _searchingCategory);
            if (notes.Count == 0)
            {
                BottomTip.Visibility = Visibility.Hidden;
                Information.Visibility = Visibility.Visible;
                Information.Text = $"There is no notes with category: \"{_searchingCategory}\"";
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
