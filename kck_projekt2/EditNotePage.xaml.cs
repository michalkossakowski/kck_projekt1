using System;
using System.Collections.Generic;
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
using MaterialDesignThemes.Wpf;

namespace kck_projekt2
{
    /// <summary>
    /// Logika interakcji dla klasy LoginPage.xaml
    /// </summary>
    public partial class EditNotePage : UserControl
    {
        private MainWindow _mainWindow;
        private int _noteId;
        private NoteController _noteController;

        public EditNotePage(MainWindow mainWindow,int noteId)
        {
            _mainWindow = mainWindow;
            _noteController = NoteController.GetInstance();
            _noteId = noteId;
            var note = _noteController.GetNoteById(_noteId);
            InitializeComponent();

            Title.Text = note.Title;
            SelectedCategory.IsEnabled = false;
            CategoryToggle.IsChecked = true;
            CustomCategory.IsEnabled = true;
            CustomCategory.Text = note.Category;
            NoteContent.Text = note.Content;
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
                    SelectedCategory.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    HintAssist.SetHelperText(CustomCategory, "Enter custom category");
                    CustomCategory.Foreground = new SolidColorBrush(Colors.Black);

                    HintAssist.SetHelperText(SelectedCategory, "You must choose category or use custom");
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
                _noteController.EditNote(_noteId, Title.Text, category, NoteContent.Text);
                _mainWindow.contentControl.Content = new ExploreNotesPage(_mainWindow);
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ExploreNotesPage(_mainWindow);
        }

        private void CategoryToggleClick(object sender, RoutedEventArgs e)
        {
            CustomCategory.IsEnabled = !CustomCategory.IsEnabled;
            SelectedCategory.IsEnabled = !SelectedCategory.IsEnabled;
        }


        private void Title_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Title.Text.Length > 0)
            {
                HintAssist.SetHelperText(Title, "Enter note title");
                Title.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void SelectedCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCategory.SelectedItem != null)
            {
                HintAssist.SetHelperText(SelectedCategory, "Select category");
                SelectedCategory.Foreground = new SolidColorBrush(Colors.Black);

                HintAssist.SetHelperText(CustomCategory, "Enter custom category");
                CustomCategory.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void CustomCategory_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CustomCategory.Text.Length > 0)
            {
                HintAssist.SetHelperText(CustomCategory, "Enter custom category");
                CustomCategory.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void NoteContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NoteContent.Text.Length > 0)
            {
                HintAssist.SetHelperText(NoteContent, "");
                NoteContent.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
