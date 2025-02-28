using System.Windows.Controls;

namespace kck_projekt2
{
    public partial class ActionMenuPage : UserControl
    {
        public ActionMenuPage(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = new ActionMenuViewModel(mainWindow);
        }
    }
}
