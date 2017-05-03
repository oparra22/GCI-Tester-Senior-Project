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
    /// Interaction logic for AddBoard.xaml
    /// </summary>
    public partial class AddBoard : Window
    {
        public List<BoardNameItem> BoardNameItems = new List<BoardNameItem>();
        String PartName = String.Empty;
        public String BoardName = String.Empty;
        public bool BestGuessPinMap = true;
        

        public AddBoard()
        {
            InitializeComponent();
            //device_ListGrid.Children
            //this.PartName = PartName;
            partName_textBox.Text = PartName;
            UpdateBoardNameList(1);
        }

        public List<String> GetNameList()
        {
            List<String> Result = new List<string>();
            foreach (BoardNameItem Item in BoardNameItems)
            {
                Result.Add(Item.DeviceName);
            }
            return Result;
        }

        void UpdateBoardNameList(int NumberOfDevices)
        {
            //device_scrollviewer.Content = null;
            device_ListGrid.Children.Clear();
            MessageBox.Show(NumberOfDevices.ToString());
            //device_ListGrid.
            BoardNameItems = new List<BoardNameItem>();
            for (int i = 0; i < NumberOfDevices; i++)
            {
                BoardNameItem Item = new BoardNameItem(i + 1, "Device" + (i + 1).ToString());
                Item.DeviceName = "Device" + (i + 1);
                //Item.Top = (i * 27) + 5;

                device_ListGrid.Children.Add(Item);
                BoardNameItems.Add(Item);
            }
           // device_ListGrid.AutoScrollMinSize = new Size(217, NumberOfDevices * 27);
        }
        
        private void upDown_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBoardNameList((int)upDown.pinValue());
           // UpdateBoardNameList(upDown);
           
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            //buttonOK.DialogResult = DialogResult.OK;
            // this.DialogResult = DialogResult.OK;
            MessageBox.Show("Board Added");
            this.Close();
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add Board Canceled!");
            this.Close();
        }

        private void bestGuess_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (bestGuess_checkBox.IsChecked == true)
                BestGuessPinMap = true;
            else
                BestGuessPinMap = false;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BoardName = textBox.Text;
        }

        
    }
}
