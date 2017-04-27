using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GCITester
{
    /// <summary>
    /// Interaction logic for itemProductionInfo.xaml
    /// </summary>
    public partial class itemProductionInfo : UserControl
    {

        bool _Result = true;

        public int SocketIndex = 0;
        public int TestBoardID = 0;
        string _SocketName = string.Empty;
        bool _SkipTest = false;
        public List<int> PinsToTest;

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

        private void checkSkip_Checked(object sender, EventArgs e)
        {
            _SkipTest = true;
            //labelResult.Visibility = Visibility.Visible;

        }

        public itemProductionInfo(String SocketName, int SocketIndex, int TestBoardID, List<int> PinsToTest)
        {
            InitializeComponent();
            this.SocketName = SocketName;
            this.SocketIndex = SocketIndex;
            this.TestBoardID = TestBoardID;
            this.PinsToTest = PinsToTest;
            //For debugging purposes
            //labelResult.Visibility = Visibility.Hidden;
        }

      

        private void labelResult_Click(object sender, EventArgs e)
        {

        }

    }
}
