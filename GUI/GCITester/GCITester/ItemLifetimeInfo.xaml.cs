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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GCITester
{
    /// <summary>
    /// Interaction logic for ItemLifetimeInfo.xaml
    /// </summary>
    public partial class ItemLifetimeInfo : UserControl
    {
        bool _Result = true;

        public int SocketIndex = 0;
        public int TestBoardID = 0;
        string _SocketName = string.Empty;
        string _SerialNumber = string.Empty;
        bool _SkipTest = false;
        public List<int> PinsToTest;
        List<String> SerialNumbers = new List<string>();

        public void SetSerialNumberList(List<String> Items)
        {
            comboSerialNumber.Items.Clear();
            foreach (String SerialNumber in Items)
            {
                comboSerialNumber.Items.Add(SerialNumber);
            }
        }




        public bool SkipTest
        {
            get { return _SkipTest; }
            //set { _SkipTest = value; }
        }

        public string SocketName
        {
            get { return _SocketName; }
            set
            {
                _SocketName = value;
                labelDeviceSlot.Content = _SocketName;
            }
        }

        public string SerialNumber
        {
            get { return _SerialNumber; }
            set
            {
                _SerialNumber = value;
                comboSerialNumber.Text = _SerialNumber;
            }
        }

        public void SetBackColor()
        {
         
                /*if (SkipTest == true)
                {
                    this.BackColor = Color.LightGray;
                }*/

                if (labelResult.Visibility == Visibility.Visible)
                {
                    if (_Result == true)
                    {
                        this.Background = Brushes.Green;
                    }
                    else
                    {
                        this.Background = Brushes.Red;
                    }
                }
                else
                {
                    this.Background = Brushes.LightGray;
                }
            
        }

        public bool Result
        {
            get { return _Result; }
            set
            {
                _Result = value;
                if (_Result == true)
                {
                    
                   labelResult.Content = "PASS";
                    
                }
                else
                {
                    
                   labelResult.Content = "FAIL";
                    
                }
            }
        }

        public void DisplayResult(bool Display)
        {

            if (SkipTest == false)
            {
                if (Display)
                {
                    labelResult.Visibility = Visibility.Visible;
                    SetBackColor();

                }
                else labelResult.Visibility = Visibility.Hidden;

            }
            //This was used for debug
            //this.Background = Brushes.Red;
        }

        public void ResetTest()
        {
            this.Background = Brushes.LightGray;
            Result = true;
            labelResult.Visibility = Visibility.Hidden;
        }

        private void textSerialNumber_TextChanged(object sender, EventArgs e)
        {
            _SerialNumber = comboSerialNumber.Text;
        }



        private void PopulateSerialNumbers()
        {
            comboSerialNumber.Items.Clear();
            foreach (String item in SerialNumbers)
            {
                comboSerialNumber.Items.Add(item);
            }
        }

        public ItemLifetimeInfo(String SocketName, int SocketIndex, int TestBoardID, List<int> PinsToTest, List<String> SerialNumbers)
        {
            InitializeComponent();
            this.SocketName = SocketName;
            this.SocketIndex = SocketIndex;
            this.TestBoardID = TestBoardID;
            this.PinsToTest = PinsToTest;
            this.SerialNumbers = SerialNumbers;
            PopulateSerialNumbers();
        }

        private void checkSkip_Checked(object sender, EventArgs e)
        {
            _SkipTest = true;
            comboSerialNumber.IsEnabled = !_SkipTest;
        }

        private void comboSerialNumber_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboSerialNumber_TextChanged(object sender, EventArgs e)
        {
            _SerialNumber = comboSerialNumber.Text;
        }
    }
}

