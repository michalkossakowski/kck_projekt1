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
    public partial class ExploreNotesByDayPage : UserControl
    {
        private MainWindow _mainWindow;
        public DateTime _date;
        public ObservableCollection<NoteModel> Notes { get; set; }
        public ExploreNotesByDayPage(MainWindow mainWindow, DateTime date)
        {
            InitializeComponent();
            _date = date;
            _mainWindow = mainWindow;

            HeaderTitle.Text = $"{date.Day}.{date.Month}.{date.Year}";
            var noteController = NoteController.GetInstance();
            var notes = noteController.GetNotesByUserIdAndDay(_mainWindow.loggedUserId, date);
            if(notes.Count == 0)
            {
                Information.Text = $"You don't have any notes from: {date.Day}.{date.Month}.{date.Year}";
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

        private void OpenEditPage(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is NoteModel note)
            {
                _mainWindow.contentControl.Content = new EditNotePage(_mainWindow,note.Id, this);
            }
        }


        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new CalendarPage(_mainWindow);
        }

    }
}
