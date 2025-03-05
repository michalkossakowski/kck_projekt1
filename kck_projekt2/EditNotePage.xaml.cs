using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using kck_api.Controller;
using kck_api.Model;
using MaterialDesignThemes.Wpf;

namespace kck_projekt2
{
    public partial class EditNotePage : UserControl
    {
        private MainWindow _mainWindow;
        private int _noteId;
        private NoteController _noteController;
        private UserControl _previousAction;
        private CategoryController _categoryController;
        public EditNotePage(MainWindow mainWindow, int noteId, UserControl previousAction)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            _noteController = NoteController.GetInstance();
            _categoryController = CategoryController.GetInstance();
            _previousAction = previousAction;
            _noteId = noteId;

            InitializeNote();
        }

        private async void InitializeNote()
        {
            var note = await _noteController.GetNoteByIdAsync(_noteId);

            Title.Text = note.Title;
            NoteContent.Text = note.Content;

            var categoryController = CategoryController.GetInstance();
            string categoryName = await categoryController.GetCategoryNameByIdAsync(note.CategoryId); 

            CustomCategory.Text = categoryName;

            CategoryToggle.IsChecked = true;
            SelectedCategory.IsEnabled = false;
            CustomCategory.IsEnabled = true;

            SelectedCategory.Visibility = Visibility.Collapsed;
            CustomCategory.Visibility = Visibility.Visible;
            await LoadCategoriesAsync();
        }

        public async Task LoadCategoriesAsync()
        {
            var categories = await _categoryController.GetAllCategoriesAsync();
            SelectedCategory.ItemsSource = categories;
        }
        private async void SaveNoteClick(object sender, RoutedEventArgs e)
        {

            var selectedCategory = SelectedCategory.SelectedItem as CategoryModel;
            string category = CustomCategory.IsEnabled
                ? CustomCategory.Text
                : selectedCategory?.Name ?? "";
            int categoryId = await _categoryController.GetOrCreateCategoryIdAsync(category);

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

            if ((!string.IsNullOrWhiteSpace(Title.Text)) && (category.Length > 0) && (NoteContent.Text.Length > 0))
            {
                var noteController = NoteController.GetInstance();
                var note = new NoteModel(_mainWindow.loggedUserId, Title.Text, NoteContent.Text, categoryId);
                await _noteController.EditNoteAsync(_noteId, Title.Text, categoryId, NoteContent.Text);

                YesNoDialog dialog = new YesNoDialog((string)Application.Current.Resources["SaveChangesConfirmStr"]);
                dialog.Owner = _mainWindow;
                if (dialog.ShowDialog() == true)
                {
                    _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                    _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                    _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["NoteEditSuccesStr"]);
                    Back();
                }
                else
                {
                    _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                    _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                    _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["NoteEditCancelStr"]);

                }
            }
            else
            {
                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.DarkRed);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["RequirementsNotMeetStr"]);
            }
        }

        public async void DeleteNoteClick(object sender, RoutedEventArgs e)
        {
            YesNoDialog dialog = new YesNoDialog((string)Application.Current.Resources["DeleteConfirmStr"]);
            dialog.Owner = _mainWindow;
            if (dialog.ShowDialog() == true)
            {
                var noteController = NoteController.GetInstance();
                await _noteController.RemoveNoteAsync(_noteId);

                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["NoteDeleteSuccesStr"]);
                Back();
            }
            else
            {
                _mainWindow.Snackbar.Background = new SolidColorBrush(Colors.Green);
                _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
                _mainWindow.Snackbar.MessageQueue?.Enqueue((string)Application.Current.Resources["NoteDeleteCancelStr"]);
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
        private async void PrintNote_Click(object sender, RoutedEventArgs e)
        {
            // Dane przykładowe do wydruku

            var note = await _noteController.GetNoteByIdAsync(_noteId);
            var categoryController = CategoryController.GetInstance();
            string categoryName = await categoryController.GetCategoryNameByIdAsync(note.CategoryId);
            // Tworzenie dokumentu do wydruku
            FlowDocument doc = GenerateFlowDocument(note.Title, categoryName, note.Content, note.ModifiedDate);

            // Wywołanie okna drukowania
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Sprawdzenie, czy wybrano drukarkę "Microsoft Print to PDF"
                PrintQueue printQueue = printDialog.PrintQueue;
                if (!printQueue.Name.Contains("Microsoft Print to PDF"))
                {
                    MessageBox.Show((string)Application.Current.Resources["ChoosePrinterStr"], (string)Application.Current.Resources["ErrorStr"], MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Drukowanie dokumentu
                DocumentPaginator paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
                printDialog.PrintDocument(paginator, "Note");
            }
        }

        private FlowDocument GenerateFlowDocument(string noteTitle, string noteCategory, string noteContent, DateTime noteModifiedDate)
        {
            // Tworzenie FlowDocument
            FlowDocument doc = new FlowDocument
            {
                PageWidth = 500,
                FontFamily = new FontFamily("Arial"),
                FontSize = 14,
                PagePadding = new Thickness(20)
            };

            // Nagłówek
            Paragraph title = new Paragraph(new Run(noteTitle))
            {
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            };

            // Kategoria
            Paragraph category = new Paragraph(new Run($"{(string)Application.Current.Resources["CategoryStr"]} {noteCategory}"))
            {
                FontSize = 14,
                FontStyle = FontStyles.Italic,
                Foreground = Brushes.Gray,
                Margin = new Thickness(0, 5, 0, 10)
            };

            // Treść notatki
            Paragraph content = new Paragraph(new Run(noteContent))
            {
                FontSize = 14,
                TextAlignment = TextAlignment.Left
            };

            // Data utworzenia
            Paragraph date = new Paragraph(new Run($"{(string)Application.Current.Resources["ModificationDateStr"]} {noteModifiedDate:g}"))
            {
                FontSize = 12,
                Foreground = Brushes.DarkGray,
                Margin = new Thickness(0, 10, 0, 0),
                TextAlignment = TextAlignment.Right
            };

            // Dodajemy elementy do dokumentu
            doc.Blocks.Add(title);
            doc.Blocks.Add(category);
            doc.Blocks.Add(content);
            doc.Blocks.Add(date);

            return doc;
        }
    }
}
