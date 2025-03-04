using System.Windows.Input;

namespace kck_projekt2
{
    public class ActionMenuViewModel : BaseViewModel
    {
        private readonly MainWindow _mainWindow;

        public ICommand OpenAddNotePageCommand { get; }
        public ICommand OpenLatestNotesPageCommand { get; }
        public ICommand OpenExploreNotesPageCommand { get; }
        public ICommand OpenCalendarPageCommand { get; }
        public ICommand OpenSearchPageCommand { get; }
        public ICommand OpenFindByDatePageCommand { get; }
        public ICommand OpenFindByCategoryPageCommand { get; }
        public ICommand OpenExploreNotesByMonthPageCommand { get; }
        public ICommand OpenManageCategoriesPageCommand { get; }
        public ActionMenuViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            OpenAddNotePageCommand = new RelayCommand(OpenAddNotePage);
            OpenLatestNotesPageCommand = new RelayCommand(OpenLatestNotesPage);
            OpenExploreNotesPageCommand = new RelayCommand(OpenExploreNotesPage);
            OpenCalendarPageCommand = new RelayCommand(OpenCalendarPage);
            OpenSearchPageCommand = new RelayCommand(OpenSearchPage);
            OpenFindByDatePageCommand = new RelayCommand(OpenFindByDatePage);
            OpenFindByCategoryPageCommand = new RelayCommand(OpenFindByCategoryPage);
            OpenExploreNotesByMonthPageCommand = new RelayCommand(OpenExploreNotesByMonthPage);
            OpenManageCategoriesPageCommand = new RelayCommand(OpenManageCategoriesPage);
        }

        private void OpenAddNotePage() => _mainWindow.contentControl.Content = new AddNotePage(_mainWindow);
        private void OpenLatestNotesPage() => _mainWindow.contentControl.Content = new LatestNotesPage(_mainWindow);
        private void OpenExploreNotesPage() => _mainWindow.contentControl.Content = new ExploreNotesPage(_mainWindow);
        private void OpenCalendarPage() => _mainWindow.contentControl.Content = new CalendarPage(_mainWindow);
        private void OpenSearchPage() => _mainWindow.contentControl.Content = new SearchPage(_mainWindow, null);
        private void OpenFindByDatePage() => _mainWindow.contentControl.Content = new FindByDatePage(_mainWindow, null);
        private void OpenFindByCategoryPage() => _mainWindow.contentControl.Content = new FindByCategoryPage(_mainWindow, null);
        private void OpenExploreNotesByMonthPage() => _mainWindow.contentControl.Content = new ExploreNotesByMonthPage(_mainWindow, null);
        private void OpenManageCategoriesPage() => _mainWindow.contentControl.Content = new ManageCategoriesPage(_mainWindow);
    }
}
