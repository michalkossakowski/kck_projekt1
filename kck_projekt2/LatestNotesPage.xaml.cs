using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using kck_api.Controller;

namespace kck_projekt2
{
    public partial class LatestNotesPage : UserControl
    {
        private MainWindow _mainWindow;
        public ObservableCollection<NoteModel> Notes { get; set; }
        public LatestNotesPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            InitializeNotes();
        }

        private async void InitializeNotes()
        {
            var noteController = NoteController.GetInstance();
            var notes = await noteController.GetLatestNotesByUserIdAsync(_mainWindow.loggedUserId, 8);

            if (notes.Count == 0)
            {
                Information.Text = "You don't have any notes right now";
                Information.Visibility = Visibility.Visible;
                BottomTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                Information.Visibility = Visibility.Collapsed;
                Notes = new ObservableCollection<NoteModel>(notes);
                DataContext = this;
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
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
