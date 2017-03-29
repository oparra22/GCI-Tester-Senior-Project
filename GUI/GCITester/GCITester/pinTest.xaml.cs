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
    /// Interaction logic for pinTest.xaml
    /// </summary>
    public partial class pinTest : Window
    {

        public pinTest()
        {
            InitializeComponent();
        }


        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Add("Pin " + "Measured");
           // listBox.SelectedIndex = listBox.Items.Count - 1;
        }
     

       
    }
}
