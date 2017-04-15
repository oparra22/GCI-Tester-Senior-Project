using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data;

namespace GCITester
{
    /// <summary>
    /// Interaction logic for lifetimeReport.xaml
    /// </summary>
    public partial class lifetimeReport : Window
    {
        DataTable dt; 
        public lifetimeReport()
        {
            InitializeComponent();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Legacy_Click(object sender, RoutedEventArgs e)
        {
            string partname = "  ";
            string batchname = " ";
            List<string> SerialNumbers = new List<string>();
            //Show that the Button is functioning
            MessageBox.Show("Button Works");
            GCIDB.Initialize();
            GCIDB.OpenConnection();

            dt = GCIDB.GetLifetimeData(partname, batchname, SerialNumbers);
            dataGrid.DataContext = dt;
            

        }
    }
}

