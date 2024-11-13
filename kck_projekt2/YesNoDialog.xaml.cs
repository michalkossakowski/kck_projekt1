using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kck_projekt2
{
    /// <summary>
    /// Interaction logic for YesNoDialog.xaml
    /// </summary>
    public partial class YesNoDialog : Window
    {
        public YesNoDialog(string message)
        {
            InitializeComponent();
            Message.Text = message;
        }

        private void YesClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true; 
        }

        private void NoClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
