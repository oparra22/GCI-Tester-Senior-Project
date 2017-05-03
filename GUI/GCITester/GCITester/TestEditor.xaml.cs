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
    /// Interaction logic for TestEditor.xaml
    /// </summary>
    public partial class TestEditor : Window    
    {
        string SelectedPartName = string.Empty;
        string SelectedBoardName = string.Empty;
        List<TestPinEntity> LoadedTestInfo = new List<TestPinEntity>();
        public TestEditor()
        {

            InitializeComponent();


            GCIDB.Initialize();
            GCIDB.OpenConnection();
            List<string> partNames = GCIDB.GetPartList();
            part_listBox.ItemsSource = partNames;

            System.ComponentModel.ICollectionView partName_view = CollectionViewSource.GetDefaultView(part_listBox.ItemsSource);
            partName_view.Filter = partName_CustomFilter;
        }
        //filter function for partnames 
        private bool partName_CustomFilter(object obj)
        {
            if (string.IsNullOrEmpty(filterParts_textBox.Text))
            {
                return true;
            }
            else
            {
                return (obj.ToString().IndexOf(filterParts_textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"name: , board: {SelectedPartName.Length}, {SelectedBoardName.Length}" );
            if (SelectedPartName.Length > 0 && SelectedBoardName.Length > 0)
            {
             
                GCIDB.DeleteBoard(SelectedPartName, SelectedBoardName);
                GCIDB.Initialize();
                GCIDB.OpenConnection();
                List<string> partNames = GCIDB.GetPartList();
                part_listBox.ItemsSource = partNames;

                System.ComponentModel.ICollectionView partName_view = CollectionViewSource.GetDefaultView(part_listBox.ItemsSource);
                partName_view.Filter = partName_CustomFilter;
            }
        }
    
        private void filterParts_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(part_listBox.ItemsSource).Refresh();
        }

        private void part_listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (part_listBox.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Part Name!");
            }
            else
            {

                //pull batch names from the DB, then render the view for filtering
                SelectedPartName = part_listBox.SelectedItem.ToString();
                GCIDB.Initialize();
                GCIDB.OpenConnection();
                List<string> boardNames = GCIDB.GetTestBoardList(SelectedPartName);
                SelectedBoardName = string.Empty;
                testBoardList1.ClearPins();
                selectBoard_listBox.ItemsSource = boardNames;
                 
            }
        }

        private void selectBoard_listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        if(selectBoard_listBox.Items.Count > 0)
        {
            if (selectBoard_listBox.SelectedItem == null)
                return;

            if (SelectedBoardName == selectBoard_listBox.SelectedItem.ToString())
                return;
            SelectedBoardName = selectBoard_listBox.SelectedItem.ToString();

            LoadedTestInfo = GCIDB.GetTestPins(SelectedPartName, SelectedBoardName);
            testBoardList1.ClearPins();
            //  test
            String LastDeviceName = string.Empty;
            foreach(TestPinEntity Pin in LoadedTestInfo)
            {
                if (LastDeviceName.Length > 0 && (LastDeviceName != Pin.SocketName))
                {
                    testBoardList1.ExtraSpace += 20;
                }

               testBoardList1.AddPinMap(Pin);
               LastDeviceName = Pin.SocketName;
            }

            SelectedPartName = part_listBox.SelectedItem.ToString();
            GCIDB.Initialize();
            GCIDB.OpenConnection();
            List<string> boardNames = GCIDB.GetTestBoardList(SelectedPartName);
            SelectedBoardName = string.Empty;
            selectBoard_listBox.ItemsSource = boardNames;

            SelectedBoardName = selectBoard_listBox.SelectedItem.ToString();

            }

    }

        private void newBoard_button_Click(object sender, RoutedEventArgs e)
        {
            AddBoard window = new AddBoard();
            window.ShowDialog();
            //AddBoard addBoard = new AddBoard();
            //List<String> DeviceNames = window.GetNameList();

           

            if (window.boolVal == true)
            {


                List<int> DUTPins = GCIDB.GetDUTPins(SelectedPartName);
                List<String> DeviceNames = window.GetNameList();
                Byte CurrentGCIPin = 1;
                int SocketIndex = 0;


                int TestBoardID = GCIDB.GetNextTestBoardID();
                int PartID = GCIDB.GetPartID(SelectedPartName);

                testBoardList1.ClearPins();
                foreach (String Device in DeviceNames)
                {
                    foreach (byte Pin in DUTPins)
                    {

                        if (CurrentGCIPin == 48)
                            CurrentGCIPin += 4;
                        GCIDB.AddTestPinMap(TestBoardID, window.BoardName, PartID, SocketIndex, Device, Pin, CurrentGCIPin, DateTime.Now);
                        CurrentGCIPin++;
                        //testBoardList1.AddPinMap(PartID, TestBoardID, Device, Pin, CurrentGCIPin++);
                    }
                    testBoardList1.ExtraSpace += 20;
                    SocketIndex++;
                }
                //AddBoardControl window = new AddBoardControl();
                //window.ShowDialog();
            }
            List<string> boardNames = GCIDB.GetTestBoardList(SelectedPartName);
            //SelectedBoardName = string.Empty;
            selectBoard_listBox.ItemsSource = boardNames;

        }

        private void saveChanges_button_Click(object sender, RoutedEventArgs e)
        {
            List<TestPinMap> TestPins = testBoardList1.CurrentPinMap;

            foreach (TestPinMap Pin in TestPins)
            {
                if (Pin.Edited == true)
                {
                    GCIDB.ChangePinMap(Pin.TestBoardID, Pin.PartID, Pin.DUTPin, Pin.GCIPin);
                }
            }
            testBoardList1.ClearAllEdits();
            MessageBox.Show("All changes saved!");
        }
    }

 }

