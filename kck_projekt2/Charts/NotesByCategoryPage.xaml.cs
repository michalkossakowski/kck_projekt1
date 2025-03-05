using kck_projekt2.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace kck_projekt2.Charts
{
    /// <summary>
    /// Logika interakcji dla klasy NotesByCategoryPage.xaml
    /// </summary>
    public partial class NotesByCategoryPage : UserControl
    {
        private readonly MainWindow _mainWindow;

        public NotesByCategoryPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            DataContext = new NotesByCategoryViewModel(mainWindow);
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }
    }
}
