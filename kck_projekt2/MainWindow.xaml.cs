using System.Text;
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
using kck_api.Database;

namespace kck_projekt2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userController = new UserController(context);
            var user = new UserModel("userWPF","wpf");
            userController.AddUser(user);

            MessageBox.Show("User Dodany");
        }
    }
}