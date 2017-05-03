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
        int pinNum = 0;
        int lowerLimit = 0;
        string pinString;
        public bool boolVal;


        public AddBoard()
        {
            InitializeComponent();
            TestEditor testEditor = new TestEditor();

            //partName_textBox.Text = testEditor.selectBoard_listBox.SelectedItem.ToString();
        }
        //sets the lower limit 
        public void setLimit(int l)
        {
            lowerLimit = l;
        }
        public void setValue(int v)
        {
            pinNum = v;
            pinString = pinNum.ToString();
            counterBox.Text = pinString;
        }
        private void upPinButton_Click(object sender, RoutedEventArgs e)
        {
            pinNum++;
            pinString = pinNum.ToString();
            counterBox.Text = pinString;
            UpdateBoardNameList(pinNum);
        }

        //decrement pin number
        private void downPinButton_Click(object sender, RoutedEventArgs e)
        {
            if (pinNum != lowerLimit)
            {
                pinNum--;
            }
            pinString = pinNum.ToString();
            counterBox.Text = pinString;
            UpdateBoardNameList(pinNum);
        }

        public int pinValue()
        {
            return pinNum;
        }

        void UpdateBoardNameList(int NumberOfDevices)
        {
            
            device_ListGrid.Children.Clear();
            
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

        public List<String> GetNameList()
        {
            List<String> Result = new List<string>();
            foreach (BoardNameItem Item in BoardNameItems)
            {
                Result.Add(Item.DeviceName);
            }
            return Result;
        }
            

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult result = MessageBox.Show("Add This Baord?", "Add Board", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                boolVal = true;
            }
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
