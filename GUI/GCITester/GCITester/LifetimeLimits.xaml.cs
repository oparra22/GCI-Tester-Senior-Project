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
    /// Interaction logic for LifetimeLimits.xaml
    /// </summary>
    public partial class LifetimeLimits : UserControl
    {

        private double _LCL;
        private double _UCL;

        public bool Edited = false;

        bool LCLInitialized = false;
        bool UCLInitialized = false;

        public event EventHandler LimitsEdited;

        private void Limits_Edited(object sender, EventArgs e)
        {
            // invoke UserControl event here
            if (this.LimitsEdited != null) this.LimitsEdited(sender, e);
        }

        public void SetEnabled(bool Enable)
        {
            //numericUCL.Enabled = Enable;
            //numericLCL.Enabled = Enable;
        }

        public double UCL
        {
            get { return _UCL; }
            set
            {
                _UCL = value;
                numericUCL.setValue(_UCL);
            }
        }

        public double LCL
        {
            get { return _LCL; }
            set
            {
                _LCL = value;
                numericLCL.setValue(_LCL);
            }
        }

        public LifetimeLimits()
        {
            InitializeComponent();
        }

        private void numericLCL_ValueChanged(object sender, EventArgs e)
        {
            _LCL = (double)numericLCL.pinValue();
            if (LCLInitialized == true)
            {
                Edited = true;
                Limits_Edited(sender, e);
            }
            LCLInitialized = true;

        }

        private void numericUCL_ValueChanged(object sender, EventArgs e)
        {
            _UCL = (double)numericUCL.pinValue();
            if (UCLInitialized == true)
            {
                Edited = true;
                Limits_Edited(sender, e);
            }
            UCLInitialized = true;

        }
    }
}
