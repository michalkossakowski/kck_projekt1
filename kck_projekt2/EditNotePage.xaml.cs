using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using kck_api.Controller;
using MaterialDesignThemes.Wpf;

namespace kck_projekt2
{
    public partial class EditNotePage : UserControl
    {
        private MainWindow _mainWindow;
        private int _noteId;
        private NoteController _noteController;
        private UserControl _previousAction;
        public EditNotePage(MainWindow mainWindow, int noteId, UserControl previousAction)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            _noteController = NoteController.GetInstance();
            _previousAction = previousAction;
            _noteId = noteId;

            InitializeNote();
        }

        private async void InitializeNote()
        {
            var note = await _noteController.GetNoteByIdAsync(_noteId);

            Title.Text = note.Title;
            CustomCategory.Text = note.Category;
            NoteContent.Text = note.Content;

            CategoryToggle.IsChecked = true;
            SelectedCategory.IsEnabled = false;
            CustomCategory.IsEnabled = true;

            SelectedCategory.Visibility = Visibility.Collapsed;
            CustomCategory.Visibility = Visibility.Visible;
        }

        private async void SaveNoteClick(object sender, RoutedEventArgs e)
        {

            var selectedCategory = SelectedCategory.SelectedItem as ComboBoxItem;
            string category = CustomCategory.IsEnabled
                ? CustomCategory.Text
                : selectedCategory?.Content?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(Title.Text))
            {
                HintAssist.SetHelperText(Title, "Title cannot be empty");
                Title.Foreground = new SolidColorBrush(Colors.Red);
            }


            if (category.Length == 0)
            {
                if (CustomCategory.IsEnabled)
                {
                    HintAssist.SetHelperText(CustomCategory, "Category cannot be empty");
                    CustomCategory.Foreground = new SolidColorBrush(Colors.Red);

                    HintAssist.SetHelperText(SelectedCategory, "Select category");
                    SelectedCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
                }
                else
                {
                    HintAssist.SetHelperText(CustomCategory, "Enter custom category");
                    CustomCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];


                    HintAssist.SetHelperText(SelectedCategory, "Choose category or use custom");
                    SelectedCategory.Foreground = new SolidColorBrush(Colors.Red);
                }
            }

            if (NoteContent.Text.Length == 0)
            {
                HintAssist.SetHelperText(NoteContent, "Note content cannot be empty");
                NoteContent.Foreground = new SolidColorBrush(Colors.Red);
            }

            if ((!string.IsNullOrWhiteSpace(Title.Text)) && (category.Length > 0) && (NoteContent.Text.Length > 0))
            {
                var noteController = NoteController.GetInstance();
                var note = new NoteModel(_mainWindow.loggedUserId, Title.Text, NoteContent.Text, category);
                await _noteController.EditNoteAsync(_noteId, Title.Text, category, NoteContent.Text);

                YesNoDialog dialog = new YesNoDialog("Are you sure you want to save changes ?");
                dialog.Owner = _mainWindow;
                if (dialog.ShowDialog() == true)
                {
                    _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                    _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                    _mainWindow.Snackbar.MessageQueue?.Enqueue("Note successfully edited");
                    Back();
                }
                else
                {
                    _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                    _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                    _mainWindow.Snackbar.MessageQueue?.Enqueue("Note edit canceled");

                }
            }
            else
            {
                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue("Meet the form requirements and try again");
            }
        }

        public async void DeleteNoteClick(object sender, RoutedEventArgs e)
        {
            YesNoDialog dialog = new YesNoDialog("Are you sure you want to delete the note?");
            dialog.Owner = _mainWindow;
            if (dialog.ShowDialog() == true)
            {
                var noteController = NoteController.GetInstance();
                await _noteController.RemoveNoteAsync(_noteId);

                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue("Note successfully deleted");
                Back();
            }
            else
            {
                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue("Deleting note canceled");
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            Back();
        }

        private void Back()
        {
            switch (_previousAction)
            {
                case ExploreNotesByDayPage explorePage:
                    _mainWindow.contentControl.Content = Activator.CreateInstance(typeof(ExploreNotesByDayPage), _mainWindow, explorePage._date);
                    break;

                case SearchPage searchPage:
                    _mainWindow.contentControl.Content = Activator.CreateInstance(typeof(SearchPage), _mainWindow, searchPage._search);
                    break;

                case FindByCategoryPage categoryPage:
                    _mainWindow.contentControl.Content = Activator.CreateInstance(typeof(FindByCategoryPage), _mainWindow, categoryPage._searchingCategory);
                    break;

                case FindByDatePage categoryPage:
                    _mainWindow.contentControl.Content = Activator.CreateInstance(typeof(FindByDatePage), _mainWindow, categoryPage._searchingDate);
                    break;

                case ExploreNotesByMonthPage categoryPage:
                    _mainWindow.contentControl.Content = Activator.CreateInstance(typeof(ExploreNotesByMonthPage), _mainWindow, categoryPage._searchingDate);
                    break;

                default:
                    _mainWindow.contentControl.Content = Activator.CreateInstance(_previousAction.GetType(), _mainWindow);
                    break;
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
                SelectedCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
            }
            else
            {
                CustomCategory.Visibility = Visibility.Visible;
                SelectedCategory.Visibility = Visibility.Collapsed;
                CustomCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
            }
        }


        private void Title_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Title.Text.Length > 0)
            {
                HintAssist.SetHelperText(Title, "Enter note title");
                Title.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];

            }
        }

        private void SelectedCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCategory.SelectedItem != null)
            {
                HintAssist.SetHelperText(SelectedCategory, "Select category");
                SelectedCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];

                HintAssist.SetHelperText(CustomCategory, "Enter custom category");
                CustomCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];

            }
        }

        private void CustomCategory_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CustomCategory.Text.Length > 0)
            {
                HintAssist.SetHelperText(CustomCategory, "Enter custom category");
                CustomCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
            }
        }

        private void NoteContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NoteContent.Text.Length > 0)
            {
                HintAssist.SetHelperText(NoteContent, "Enter note content");
                NoteContent.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];

            }
        }
    }
}
