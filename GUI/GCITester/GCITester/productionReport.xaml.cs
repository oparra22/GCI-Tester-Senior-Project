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
using MySql.Data.MySqlClient;
using System.Data;

namespace GCITester
{
    /// <summary>
    /// Interaction logic for productionReport.xaml
    /// </summary>
    /// 
    public partial class productionReport : Window
    {
        public List<String> partNames;
        public List<String> batchNames;
        string SelectedPartName = string.Empty;
        string SelectedBatchName = string.Empty;
        DataTable dt;

    
        
        public productionReport()
        {
           
            InitializeComponent();

            //list Production reports in a given directory
            DirectoryInfo dinfo = new DirectoryInfo(@"..\..\..\..\..\Reports\Production Reports");
            FileInfo[] Files = dinfo.GetFiles();

            //file list to string for filter purposes
            var fileList = Files.ToList();

            //show all files before filtering
            listBox.ItemsSource = fileList;

            //pull parts list from the database
            GCIDB.Initialize();
            GCIDB.OpenConnection();
            partNames = GCIDB.GetPartList();
            partName_listBox1.ItemsSource = partNames;

            //create view then filter, doesnt includes batch view, which is changed in the 
            System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(listBox.ItemsSource);
            view.Filter = CustomFilter;

            System.ComponentModel.ICollectionView partName_view = CollectionViewSource.GetDefaultView(partName_listBox1.ItemsSource);
            partName_view.Filter = partName_CustomFilter;
                
        }

        //filter function for partnames 
        private bool partName_CustomFilter(object obj)
        {
            if (string.IsNullOrEmpty(filterPartName_textBox.Text))
            {
                return true;
            }
            else
            {
                return (obj.ToString().IndexOf(filterPartName_textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        //filter function for batch names 
        private bool batchName_CustomFilter(object obj)
        {
            if (string.IsNullOrEmpty(filterBatchName_textBox.Text))
            {
                return true;
            }
            else
            {
                 return (obj.ToString().IndexOf(filterBatchName_textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0); 
            }

                
        }
        //filter the file listview function
        private bool CustomFilter(object obj)
        {
            if (string.IsNullOrEmpty(filterSearchBox.Text))
            {
                return true;
            }
            else
            {
                return (obj.ToString().IndexOf(filterSearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
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
                string fullFileName = System.IO.Path.Combine(@"..\..\..\..\..\Reports\Production Reports", file);
                Process.Start(fullFileName);
            }
        }

        //grab values from text boxes to query DB for legacy data report
        private void legacyButton_Click(object sender, RoutedEventArgs e)
        {

            if (batchName_listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Batch Name!");
            }
            else
            {
                SelectedBatchName = batchName_listBox1.SelectedItem.ToString();
                MessageBox.Show(string.Format("Pulling Legacy Report for Part ID: {0}  Batch ID: {1}", SelectedPartName, SelectedBatchName));

                //connect to DB, get production data then return into datatable
                GCIDB.Initialize();
                GCIDB.OpenConnection();
                dt = GCIDB.GetProductionData(SelectedPartName, SelectedBatchName);
                dataGrid.DataContext = dt;
            }

        }

       
       
        //once user has selected the part, then they will get a list of batches after >> button press
        private void generateBatch_button_Click(object sender, RoutedEventArgs e)
        {
            if (partName_listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Part Name!");
            }
            else
            {
                //pull batch names from the DB, then render the view for filtering
                SelectedPartName = partName_listBox1.SelectedItem.ToString();
                GCIDB.Initialize();
                GCIDB.OpenConnection();
                batchNames = GCIDB.GetProductionBatchNameList(SelectedPartName);
                batchName_listBox1.ItemsSource = batchNames;
           
                System.ComponentModel.ICollectionView batchName_view = CollectionViewSource.GetDefaultView(batchName_listBox1.ItemsSource);
                batchName_view.Filter = batchName_CustomFilter;
            }
            
        }
        //textbox for filter of listview, refresh with every keystroke
        private void filterSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBox.ItemsSource).Refresh();
        }

        //text box for filtering partnames event handler
        private void filterPartName_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(partName_listBox1.ItemsSource).Refresh();
        }

        //textbox for filtering batchnames event handler
        private void filterBatchName_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(batchName_listBox1.ItemsSource).Refresh();
        }
    }
}
