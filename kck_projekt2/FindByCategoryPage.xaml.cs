﻿using System;
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
    public partial class FindByCategoryPage : UserControl
    {
        private MainWindow _mainWindow;
        public ObservableCollection<NoteModel> Notes { get; set; }
        private NoteController _noteController;
        public FindByCategoryPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _noteController = NoteController.GetInstance();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }

        private void SearchClick(object sender, RoutedEventArgs e) 
        {
            DataContext = null;
            var notes = _noteController.GetNotesByUserIdAndTitle(_mainWindow.loggedUserId, SearchInput.Text);
            if (notes.Count == 0) 
            { 
                Information.Visibility = Visibility.Visible;
                Information.Text = "There is no notes that match your search";
                Information.Foreground = new SolidColorBrush(Color.FromRgb(255,0,0));
            }
            else
            {
                BottomTip.Visibility = Visibility.Visible;
                Information.Visibility = Visibility.Collapsed;
                Notes = new ObservableCollection<NoteModel>(notes);
                DataContext = this;
            }
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
