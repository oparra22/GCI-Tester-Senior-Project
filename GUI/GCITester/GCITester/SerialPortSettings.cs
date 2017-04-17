using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace GCITester
{
    public partial class SerialPortSettings : UserControl
    {
        String _COMPort = String.Empty;

        int _BaudRate = 9600;
        int _DataBits = 8;
        StopBits _StopBit = StopBits.One;
        Parity _Parity = Parity.None;


        public String[] ComPortList
        {
            get
            {
                return SerialPort.GetPortNames();
            }
        }

        public String COMPort
        {
            get
            {
                return _COMPort;
            }
            set
            {
                _COMPort = value;
                //MessageBox.Show($"_Comport = {_COMPort}");
                //MessageBox.Show($"comboCOMPort {comboCOMPort}");
                comboCOMPort.Text = _COMPort;
            }
        }
        public int[] BaudRatesList
        {
            get
            {
                int[] baudRates = { 9600, 14400, 19200, 28800, 38400, 56000, 57600, 115200 };

                return baudRates;
            }
        }
        public int[] DataBitsList
        {
            get
            {
                int[] dataBits = { 5, 6, 7, 8 };

                return dataBits;
            }
        }

        public double[] StopBitsList
        {
            get
            {
                double[] stopBits = { 1, 1.5, 2 };

                return stopBits;
            }
        }



        public int BaudRate
        {
            get
            {
                return _BaudRate;
            }
            set
            {
                _BaudRate = value;
                comboBaudRate.Text = _BaudRate.ToString();
            }
        }

        public int DataBits
        {
            get
            {
                return _DataBits;
            }
            set
            {
                _DataBits = value;
                comboDataBits.Text = _DataBits.ToString();
            }
        }

        public StopBits StopBit
        {
            get
            {
                return _StopBit;
            }
            set
            {
                _StopBit = value;
                comboStopBits.Text = _StopBit.ToString();
            }
        }

        public Parity Parity
        {
            get
            {
                return _Parity;
            }
            set
            {
                _Parity = value;
                comboParity.Text = _Parity.ToString();
            }
        }


        public SerialPortSettings()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboCOMPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            _COMPort = comboCOMPort.Text;
        }

        private void PopulateCOMPorts()
        {
            comboCOMPort.Items.Clear();
            comboCOMPort.Items.AddRange(ComPortList);
            if (comboCOMPort.Items.Count > 0)
            {
                comboCOMPort.SelectedIndex = 0;
            }

        }

        private void SerialPortSettings_Load(object sender, EventArgs e)
        {
            PopulateCOMPorts();
        }

        private void comboBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _BaudRate = int.Parse(comboBaudRate.Text);
            }
            catch
            {
                _BaudRate = 9600;
            }
        }

        private void comboDataBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _DataBits = int.Parse(comboDataBits.Text);
            }
            catch
            {
                _DataBits = 8;
            }
        }

        private void comboParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _Parity = (Parity)Enum.Parse(typeof(Parity), comboParity.Text);
            }
            catch
            {
                _Parity = System.IO.Ports.Parity.None;
            }
        }

        private void comboStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _StopBit = (StopBits)int.Parse(comboStopBits.Text);
            }
            catch
            {
                _StopBit = StopBits.One;
            }
        }

        private void voltageRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _StopBit = (StopBits)int.Parse(comboStopBits.Text);
            }
            catch
            {
                _StopBit = StopBits.One;
            }
        }
    }

}
