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
    /// Interaction logic for tests.xaml
    /// </summary>
    public partial class tests : Window
    {
        //int portFlag; 

        public tests()
        {
            InitializeComponent();
        }

        private void pinTestButton_Click(object sender, RoutedEventArgs e)
        {
            pinTest window = new pinTest();
            window.ShowDialog();
        }

        private void shortTestButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void productionTestButton_Click(object sender, RoutedEventArgs e)
        {
            productionTest window = new productionTest();
            window.ShowDialog();
        }

        private void lifetimeTestButton_Click(object sender, RoutedEventArgs e)
        {
            lifetimeTest window = new lifetimeTest();
            window.ShowDialog();
        }

        private void productionLimits_Click(object sender, RoutedEventArgs e)
        {
            productionLimits window = new productionLimits();
            window.ShowDialog();
        }

        private void backToMainButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.testsButton.IsEnabled = true; 
            window.Show();
            this.Close();
        }

        
    }
}
