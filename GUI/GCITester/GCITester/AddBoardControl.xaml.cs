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
    /// Interaction logic for AddBoardControl.xaml
    /// </summary>
    public partial class AddBoardControl : Window
    {
        int pinNum;
        int lowerLimit = 0;
        string pinString;
        public List<BoardNameItem> BoardNameItems = new List<BoardNameItem>();


        public AddBoardControl()
        {
            InitializeComponent();
        }

        //sets the lower limit 
        public void setLimit(int l)
        {
            lowerLimit = l;
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
    }
}
