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
    public partial class BoardNameItem : UserControl
    {
        String _DeviceName = string.Empty;
        int _DeviceNumber = 0;

        public int DeviceNumber
        {
            get
            {
                return _DeviceNumber;
            }
            set
            {
                _DeviceNumber = value;
                boardName_label.Content = "Device " + _DeviceNumber.ToString();
            }
        }

        public String DeviceName
        {
            get
            {
                return _DeviceName;
            }
            set
            {
                _DeviceName = value;
                deviceName_textBox.Text = _DeviceName;
            }
        }


        public BoardNameItem(int DeviceNumber, String DeviceName)
        {
            InitializeComponent();
            this.DeviceNumber = DeviceNumber;
            this.DeviceName = DeviceName;
        }

        private void textDeviceName_TextChanged(object sender, EventArgs e)
        {
            _DeviceName = deviceName_textBox.Text;
        }
    }
}
