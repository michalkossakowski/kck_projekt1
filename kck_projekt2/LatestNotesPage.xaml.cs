using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class LatestNotesPage : UserControl
    {
        private MainWindow _mainWindow;
        public ObservableCollection<NoteModel> Notes { get; set; }
        public LatestNotesPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            var noteController = NoteController.GetInstance();
            var notes = noteController.GetLatestNotesByUserId(_mainWindow.loggedUserId, 10);

            Notes = new ObservableCollection<NoteModel>(notes);
            DataContext = this;
        }

       
        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }

        private void MaterialDesignFilledTextBoxEnabledComboBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
