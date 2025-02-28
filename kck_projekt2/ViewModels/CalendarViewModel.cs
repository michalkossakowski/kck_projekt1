using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using kck_api.Controller;
using MaterialDesignThemes.Wpf;

namespace kck_projekt2
{
    public class CalendarViewModel : BaseViewModel
    {
        private readonly MainWindow _mainWindow;
        private readonly NoteController _noteController;
        private List<NoteModel> _currentMonthNotes;
        public List<NoteModel> CurrentMonthNotes => _currentMonthNotes;
        private DateTime _selectedDay = DateTime.MinValue;
        public DateTime SelectedDay
        {
            get => _selectedDay;
            set
            {
                _selectedDay = value;
                OnPropertyChanged(nameof(SelectedDay));
            }
        }
        private DateTime _displayDateStart;
        public DateTime DisplayDateStart
        {
            get => _displayDateStart;
            private set
            {
                _displayDateStart = value;
                OnPropertyChanged(nameof(DisplayDateStart));
            }
        }
        private DateTime _displayDateEnd;
        public DateTime DisplayDateEnd
        {
            get => _displayDateEnd;
            private set
            {
                _displayDateEnd = value;
                OnPropertyChanged(nameof(DisplayDateEnd));
            }
        }
        public ICommand ShowNotesCommand { get; }
        public ICommand BackCommand { get; }

        public CalendarViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _noteController = NoteController.GetInstance();

            ShowNotesCommand = new RelayCommand(ShowNotes);
            BackCommand = new RelayCommand(Back);

            InitializeCalendar();
            InitializeNotes();
        }

        private async void InitializeNotes()
        {
            _currentMonthNotes = await _noteController.GetNotesByUserIdAndMonthAsync(_mainWindow.loggedUserId, DateTime.Now);
        }

        private void InitializeCalendar()
        {
            DateTime now = DateTime.Now;
            DisplayDateStart = new DateTime(now.Year, now.Month, 1); 
            DisplayDateEnd = DisplayDateStart.AddMonths(1).AddDays(-1); 
        }

        private void ShowNotes()
        {
            if (SelectedDay == DateTime.MinValue)
            {
                ShowSnackbar("Select date in the calendar to show notes", Colors.DarkRed);
            }
            else
            {
                _mainWindow.contentControl.Content = new ExploreNotesByDayPage(_mainWindow, SelectedDay);
            }
        }

        private void Back()
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }

        private void ShowSnackbar(string message, Color color)
        {
            _mainWindow.Snackbar.Background = new SolidColorBrush(color);
            _mainWindow.Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
            _mainWindow.Snackbar.MessageQueue?.Enqueue(message);
        }
    }
}