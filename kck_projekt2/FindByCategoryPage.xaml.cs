using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using kck_api.Controller;
using kck_api.Model;

namespace kck_projekt2
{
    public partial class FindByCategoryPage : UserControl
    {
        private MainWindow _mainWindow;
        public ObservableCollection<NoteModel> Notes { get; set; }
        private NoteController _noteController;
        private CategoryController _categoryController;
        public string _searchingCategory;
        public FindByCategoryPage(MainWindow mainWindow, string? searchingCategory)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _noteController = NoteController.GetInstance();
            _categoryController = CategoryController.GetInstance();
            LoadCategoriesAsync();
            if (searchingCategory != null)
            {
                SelectedCategory.IsEnabled = false;
                SelectedCategory.Visibility = Visibility.Collapsed;
                CustomCategory.IsEnabled = true;
                CustomCategory.Visibility = Visibility.Visible;
                CustomCategory.Text = searchingCategory;
                _searchingCategory = searchingCategory;
                Search();
            }
        }
        private async void LoadCategoriesAsync()
        {
            SelectedCategory.ItemsSource = await _categoryController.GetAllCategoriesAsync();
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            var searchingCategory = SelectedCategory.SelectedItem as CategoryModel;
            string category = CustomCategory.IsEnabled
                ? CustomCategory.Text
                : searchingCategory?.Name ?? "";
            _searchingCategory = category;

            if (_searchingCategory.Length == 0)
            {
                Information.Visibility = Visibility.Visible;
                Information.Text = (string)Application.Current.Resources["EmptyCategorySearchStr"];
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
            int searchingCategoryId = await _categoryController.GetOrCreateCategoryIdAsync(_searchingCategory);
            var notes = await _noteController.GetNotesByUserIdAndCategoryAsync(_mainWindow.loggedUserId, searchingCategoryId);
            if (notes.Count == 0)
            {
                BottomTip.Visibility = Visibility.Hidden;
                Information.Visibility = Visibility.Visible;
                Information.Text = (string)Application.Current.Resources["NoCategorySearchResultsStr"] + $" \"{_searchingCategory}\"";
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

        private void CategoryToggleClick(object sender, RoutedEventArgs e)
        {
            CustomCategory.IsEnabled = !CustomCategory.IsEnabled;
            SelectedCategory.IsEnabled = !SelectedCategory.IsEnabled;
            if (SelectedCategory.IsEnabled)
            {
                CustomCategory.Visibility = Visibility.Collapsed;
                SelectedCategory.Visibility = Visibility.Visible;
            }
            else
            {
                CustomCategory.Visibility = Visibility.Visible;
                SelectedCategory.Visibility = Visibility.Collapsed;
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
