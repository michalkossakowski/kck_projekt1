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
            CategoryCheckBox.IsChecked = true;
            CustomCategory.IsEnabled = true;
            CustomCategory.Text = note.Category;
            NoteContent.Text = note.Content;
        }

        private async void SaveNoteClick(object sender, RoutedEventArgs e)
        {
            var selectedCategory = SelectedCategory.SelectedItem as ComboBoxItem;
            string categoryValue = SelectedCategory.IsEnabled ? selectedCategory.Content.ToString() : CustomCategory.Text;
            _noteController.EditNote(_noteId, Title.Text, categoryValue,NoteContent.Text);
            _mainWindow.contentControl.Content = new ExploreNotesPage(_mainWindow);
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ExploreNotesPage(_mainWindow);
        }

        private void ChangeCategoryTypeClick(object sender, RoutedEventArgs e)
        {
            CustomCategory.IsEnabled = !CustomCategory.IsEnabled;
            SelectedCategory.IsEnabled = !SelectedCategory.IsEnabled;
        }
    }
}
