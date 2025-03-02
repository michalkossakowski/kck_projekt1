using kck_api.Controller;
using kck_projekt2.ViewModels;
using System.Buffers;
using System.Windows;
using System.Windows.Controls;

namespace kck_projekt2
{
    public partial class NotesInMonthPage : UserControl
    {
        private readonly MainWindow _mainWindow;

        public NotesInMonthPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            DataContext = new NotesInMonthViewModel(mainWindow);
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }
    }
}
