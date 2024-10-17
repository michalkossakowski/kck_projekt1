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
            string titleValue = Title.Text;
            var selectedCategory = SelectedCategory.SelectedItem as ComboBoxItem;
            string categoryValue = SelectedCategory.IsEnabled ? selectedCategory.Content.ToString() : CustomCategory.Text;
            string contentValue = NoteContent.Text;

            var noteController = NoteController.GetInstance();

            var note = new NoteModel(_mainWindow.loggedUserId, titleValue, contentValue, categoryValue);
            noteController.AddNote(note);

            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);

            MessageBox.Show($"Dodano notatkę \nTitle:{note.Title} \nCategory:{note.Category} \nContent:{note.Content}");
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }

        private void ChangeCategoryTypeClick(object sender, RoutedEventArgs e)
        {
            CustomCategory.IsEnabled = !CustomCategory.IsEnabled;
            SelectedCategory.IsEnabled = !SelectedCategory.IsEnabled;
        }


        private void MaterialDesignFilledTextBoxEnabledComboBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
