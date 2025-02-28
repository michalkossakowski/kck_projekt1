using System.Windows;

namespace kck_projekt2
{
    public partial class YesNoDialog : Window
    {
        public YesNoDialog(string message)
        {
            InitializeComponent();
            DataContext = new YesNoDialogViewModel(this, message);
        }
    }
}