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

using System.ComponentModel;
using System.Drawing;
using System.Data;
//using System.Windows.Forms;

namespace GCITester
{
    public partial class TestPinMap : UserControl
    {
        private String _DeviceName = String.Empty;
        private int _DUTPin = 0;
        public int _GCITesterPin = 0;
        public int TestBoardID = 0;
        public int PartID = 0;
        public bool Edited = false;

        public String DeviceName
        {
            get
            {
                return _DeviceName;
            }
            set
            {
                _DeviceName = value;
                labelSocketName.Content = _DeviceName;
            }
        }

        public int DUTPin
        {
            get
            {
                return _DUTPin;
            }
            set
            {
                _DUTPin = value;
                labelDUTPin.Content = "DUT PIN " + _DUTPin.ToString();
            }
        }

        public int GCIPin
        {
            get
            {
                return _GCITesterPin;
            }
            set
            {
                _GCITesterPin = value;
                comboGCITesterPin.SelectedIndex = _GCITesterPin - 1;
            }
        }

        void PopulateGCITesterPins()
        {
            for (int i = 1; i <= 105; i++)
            {
                comboGCITesterPin.Items.Add(i.ToString());
            }
        }

        public TestPinMap(int PartID, int TestBoardID, String DeviceName, int DUTPin, int GCITesterPin)
        {
            InitializeComponent();
            PopulateGCITesterPins();
            this.PartID = PartID;
            this.TestBoardID = TestBoardID;
            this.DeviceName = DeviceName;
            this.DUTPin = DUTPin;
            this.GCIPin = GCITesterPin;


        }

        private void comboGCITesterPin_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int Pin = int.Parse(comboGCITesterPin.Text);
                if (Pin != _GCITesterPin)
                    Edited = true;
                _GCITesterPin = Pin;

            }
            catch (Exception ex)
            {
            }
        }
    }
}

