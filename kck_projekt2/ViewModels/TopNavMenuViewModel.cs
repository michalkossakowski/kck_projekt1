using kck_projekt2.Charts;
using System.Windows.Input;

namespace kck_projekt2.ViewModels
{
    public class TopNavMenuViewModel: BaseViewModel
    {

        private readonly MainWindow _mainWindow;

        public ICommand OpenNotesInMonthPageCommand { get; }
        public ICommand OpenNotesByCategoryCommand { get; }

        public TopNavMenuViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            OpenNotesInMonthPageCommand = new RelayCommand(OpenNotesInMonthPage);
            OpenNotesByCategoryCommand = new RelayCommand(OpenNotesByCategoryPage);

        }

        private void OpenNotesInMonthPage() => _mainWindow.contentControl.Content = new NotesInMonthPage(_mainWindow);
        private void OpenNotesByCategoryPage() => _mainWindow.contentControl.Content = new NotesByCategoryPage(_mainWindow);
    }
}
