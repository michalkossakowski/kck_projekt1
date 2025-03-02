using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using kck_api.Controller;
using MaterialDesignThemes.Wpf;

namespace kck_projekt2
{
    public partial class AddNotePage : UserControl
    {
        private MainWindow _mainWindow;

        public AddNotePage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private async void AddNoteClick(object sender, RoutedEventArgs e)
        {
            var selectedCategory = SelectedCategory.SelectedItem as ComboBoxItem;
            string category = CustomCategory.IsEnabled
                ? CustomCategory.Text
                : selectedCategory?.Content?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(Title.Text))
            {
                HintAssist.SetHelperText(Title, (string)Application.Current.Resources["TitleEmptyStr"]);
                Title.Foreground = new SolidColorBrush(Colors.Red);
            }


            if (category.Length == 0)
            {
                if (CustomCategory.IsEnabled)
                {
                    HintAssist.SetHelperText(CustomCategory, (string)Application.Current.Resources["CategoryEmptyStr"]);
                    CustomCategory.Foreground = new SolidColorBrush(Colors.Red);

                    HintAssist.SetHelperText(SelectedCategory, (string)Application.Current.Resources["CategoryHelperStr"]);
                    SelectedCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
                }
                else
                {
                    HintAssist.SetHelperText(CustomCategory, (string)Application.Current.Resources["CustomCategoryHelperStr"]);
                    CustomCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];


                    HintAssist.SetHelperText(SelectedCategory, (string)Application.Current.Resources["NoCategoryChosedStr"]);
                    SelectedCategory.Foreground = new SolidColorBrush(Colors.Red);
                }
            }

            if (NoteContent.Text.Length == 0)
            {
                HintAssist.SetHelperText(NoteContent, (string)Application.Current.Resources["ContentEmptyStr"]);
                NoteContent.Foreground = new SolidColorBrush(Colors.Red);
            }


            if((!string.IsNullOrWhiteSpace(Title.Text)) && (category.Length > 0) && (NoteContent.Text.Length > 0))
            {
                var noteController = NoteController.GetInstance();
                var note = new NoteModel(_mainWindow.loggedUserId, Title.Text, NoteContent.Text, category);
                noteController.AddNoteAsync(note);
                _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["NoteCreationSuccesStr"]);
            }
            else
            {
                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["RequirementsNotMeetStr"]);
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
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
                HintAssist.SetHelperText(Title, (string)Application.Current.Resources["TitleHelperStr"]);
                Title.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];

            }
        }

        private void SelectedCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCategory.SelectedItem != null)
            {
                HintAssist.SetHelperText(SelectedCategory, (string)Application.Current.Resources["CategoryHelperStr"]);
                SelectedCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];

                HintAssist.SetHelperText(CustomCategory, (string)Application.Current.Resources["CustomCategoryHelperStr"]);
                CustomCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];

            }
        }

        private void CustomCategory_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CustomCategory.Text.Length > 0)
            {
                HintAssist.SetHelperText(CustomCategory, (string)Application.Current.Resources["CustomCategoryHelperStr"]);
                CustomCategory.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
            }
        }

        private void NoteContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NoteContent.Text.Length > 0)
            {
                HintAssist.SetHelperText(NoteContent, (string)Application.Current.Resources["ContentHelperStr"]);
                NoteContent.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];

            }
        }
    }
}
