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
using System.IO;
using System.Diagnostics;


namespace GCITester
{
    
    /// <summary>
    /// Interaction logic for lifetimeReport.xaml
    /// </summary>
    public partial class lifetimeReport : Window
    {
        
        public List<String> partNames;
        public List<String> batchNames;
        public List<String> SerialNumbers;
        string SelectedPartName = string.Empty;
        string SelectedBatchName = string.Empty;
        List<String> SelectedSerialNumbers;
        DataTable dt; 
        public lifetimeReport()
        {
            InitializeComponent();

            DirectoryInfo dinfo = new DirectoryInfo(@"..\..\..\..\..\Reports\Lifetime Reports");
            FileInfo[] Files = dinfo.GetFiles();

            //file list to string for filter purposes
            var fileList = Files.ToList();

            //show all files before filtering
            files_listbox.ItemsSource = fileList;

            //get part names
            GCIDB.Initialize();
            GCIDB.OpenConnection();
            partNames = GCIDB.GetPartList();
            partName_listbox.ItemsSource = partNames;

            System.ComponentModel.ICollectionView file_view = CollectionViewSource.GetDefaultView(files_listbox.ItemsSource);
            file_view.Filter = files_CustomFilter;

            //filter for partnames
            System.ComponentModel.ICollectionView partName_view = CollectionViewSource.GetDefaultView(partName_listbox.ItemsSource);
            partName_view.Filter = partName_CustomFilter;
        }

        private bool files_CustomFilter(object obj)
        {
            if (string.IsNullOrEmpty(filterFiles_textBox.Text))
            {
                return true;
            }
            else
            {
                return (obj.ToString().IndexOf(filterFiles_textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private bool partName_CustomFilter(object obj)
        {
            if (string.IsNullOrEmpty(partName_textBox.Text))
            {
                return true;
            }
            else
            {
                return (obj.ToString().IndexOf(partName_textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private bool batchName_CustomFilter(object obj)
        {
            if (string.IsNullOrEmpty(batchName_textBox.Text))
            {
                return true;
            }
            else
            {
                return (obj.ToString().IndexOf(batchName_textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }


        }

        private bool serialNumber_CustomFilter(object obj)
        {
            if (string.IsNullOrEmpty(serialNumbersFilter_textBox.Text))
            {
                return true;
            }
            else
            {
                return (obj.ToString().IndexOf(serialNumbersFilter_textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }


        }

        private void Legacy_Click(object sender, RoutedEventArgs e)
        {
            
            List<string> SerialNumbers = new List<string>();
            //Show that the Button is functioning
            MessageBox.Show(string.Format("Pulling Legacy Report for Part ID: {0}  Batch ID: {1} and Serial Number: {2}", SelectedPartName, SelectedBatchName, SelectedSerialNumbers));
            GCIDB.Initialize();
            GCIDB.OpenConnection();

            dt = GCIDB.GetLifetimeData(SelectedPartName, SelectedBatchName, SelectedSerialNumbers);
            dataGrid.DataContext = dt;
            

        }

        private void batchName_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(batchName_listbox.ItemsSource).Refresh();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //function here to rid of error
        }

        private void partName_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(partName_listbox.ItemsSource).Refresh();
        }

        private void getBatches_button_Click(object sender, RoutedEventArgs e)
        {
            if (partName_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Part Name!");
            }
            else
            {
                //pull batch names from the DB, then render the view for filtering
                SelectedPartName = partName_listbox.SelectedItem.ToString();
                GCIDB.Initialize();
                GCIDB.OpenConnection();
                batchNames = GCIDB.GetBatchNameList(SelectedPartName);
                batchName_listbox.ItemsSource = batchNames;

                System.ComponentModel.ICollectionView batchName_view = CollectionViewSource.GetDefaultView(batchName_listbox.ItemsSource);
                batchName_view.Filter = batchName_CustomFilter;
            }
        }

        private void getSerials_button_Click(object sender, RoutedEventArgs e)
        {
            if (batchName_listbox.SelectedIndex == -1 || partName_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Batch Name & Part Name!");
            }
            else
            {
                //pull batch names from the DB, then render the view for filtering
                SelectedPartName = partName_listbox.SelectedItem.ToString();
                SelectedBatchName = batchName_listbox.SelectedItem.ToString();
                GCIDB.Initialize();
                GCIDB.OpenConnection();
                SerialNumbers = GCIDB.GetSerialNumberList(SelectedPartName, SelectedBatchName);
                serialNumbers_listBox.ItemsSource = SerialNumbers;

                System.ComponentModel.ICollectionView serial_view = CollectionViewSource.GetDefaultView(serialNumbers_listBox.ItemsSource);
                serial_view.Filter = serialNumber_CustomFilter;
            }
        }

        private void serialNumbersFilter_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(serialNumbers_listBox.ItemsSource).Refresh();
        }

        private void filterFiles_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(files_listbox.ItemsSource).Refresh();
        }

        private void openReport_button_Click(object sender, RoutedEventArgs e)
        {
            if (files_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a file!");
            }
            else
            {
                string file = files_listbox.SelectedItem.ToString();
                string fullFileName = System.IO.Path.Combine(@"..\..\..\..\..\Reports\Lifetime Reports", file);
                Process.Start(fullFileName);
            }
        }

        List<String> GetSelectedSerialNumbers()
        {
            List<String> Result = new List<string>();
            foreach (Object selecteditem in serialNumbers_listBox.SelectedItems)
            {
                Result.Add(selecteditem.ToString());
            }
            return Result;
        }

        private void generateReort_button_Click(object sender, RoutedEventArgs e)
        {
            List<String> SerialNumbers = GetSelectedSerialNumbers();
            if (SerialNumbers.Count == 0)
            {
                MessageBox.Show("Please select serial numbers.");
                return;
            }
            DataTable dtResult = GCIDB.GetLifetimeData(SelectedPartName, SelectedBatchName, SerialNumbers);
            LifetimeLimitEntity Limits = GCIDB.GetLifetimeLimits(SelectedPartName);
            LifeTimeReportData LifetimeReport = new LifeTimeReportData(dtResult, Limits);
            LifetimeReport.GenerateExcelOutput(customerName_textBox.Text, PO_textBox.Text, prodDesc_textBox.Text, SelectedPartName, SelectedBatchName);
            //ExportToExcel.FastExportToExcel(dtResult);
        }
    }
}

