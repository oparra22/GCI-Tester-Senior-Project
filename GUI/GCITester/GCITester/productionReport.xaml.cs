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
using System.IO;
using System.Diagnostics;

namespace GCITester
{
    /// <summary>
    /// Interaction logic for productionReport.xaml
    /// </summary>
    /// 
    public partial class productionReport : Window
    {
        //list Production reports in a given directory
        public productionReport()
        {
            InitializeComponent();
            DirectoryInfo dinfo = new DirectoryInfo(@"..\..\Reports\Production Reports");
            FileInfo[] Files = dinfo.GetFiles();
           
            foreach (FileInfo file in Files)
            {
                listBox.Items.Add(file);
            }
        }
     
        //button press to open the report selected
        private void reportButton_Click_1(object sender, RoutedEventArgs e)
        { 
            if(listBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a file!");
            }
            else
            {
                string file = listBox.SelectedItem.ToString();
                string fullFileName = System.IO.Path.Combine(@"..\..\Reports\Production Reports", file);
                Process.Start(fullFileName);
            }
        }

        //grab values from text boxes to query DB for legacy data report
        private void legacyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Works");

        }

        private void filterSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
