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
        DataTable dt;
        //list Production reports in a given directory
        public productionReport()
        {
           
            InitializeComponent();
            DirectoryInfo dinfo = new DirectoryInfo(@"..\..\..\..\..\Reports\Production Reports");
            FileInfo[] Files = dinfo.GetFiles();

            //file list to string for filter purposes
            var fileList = Files.ToList();
            
            //show all files before filtering
            listBox.ItemsSource = fileList;
            
            //create view then filter
            System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(listBox.ItemsSource);
            view.Filter = CustomFilter;
        }

        //filter the listview function
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
            //string partID = partID_textbox.Text;
            //string testDataEntryID = testDataID_testbox.Text;
            string partID ="";
            string BatchID = "";
            //MessageBox.Show(string.Format("Pulling Legacy Report for Part ID: {0}  Test Entry ID: {1}", partID, testDataEntryID));


            GCIDB.Initialize();
            //string connectionString = "SERVER=localhost;PORT=3306;DATABASE=gci;UID=root;PASSWORD=root";

            //MySqlConnection connection = new MySqlConnection(connectionString);
            //GCIDB.GetProductionData(partID, testDataEntryID);
             //MySqlCommand cmd = new MySqlCommand("SELECT * from productiondata", connection);

            //DataGrid dt = new DataGrid;

            GCIDB.OpenConnection();
          
           // connection.Open();
            //DataTable dt = new DataTable();
            //dt.Load(cmd.ExecuteReader());
           // connection.Close();
            dt = GCIDB.GetProductionData(partID,BatchID);

            dataGrid.DataContext = dt;


            //MySqlConnection connection = new MySqlConnection
            

        }

        //textbox for filter of listview, refresh with every keystroke
        private void filterSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          CollectionViewSource.GetDefaultView(listBox.ItemsSource).Refresh();
        }
    }
}
