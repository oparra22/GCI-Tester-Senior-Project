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

namespace GCITester
{
    /// <summary>
    /// Interaction logic for reports.xaml
    /// </summary>
    public partial class reports : Window
    {
     //   int portFlag; 
        public reports()
        {
            InitializeComponent();
        }

        private void productionReport_Click(object sender, RoutedEventArgs e)
        {
          //  portFlag = 1;
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void lifetimeReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void backToMain_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
