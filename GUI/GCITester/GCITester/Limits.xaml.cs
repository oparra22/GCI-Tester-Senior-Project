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
    /// Interaction logic for Limits.xaml
    /// </summary>
    public partial class Limits : UserControl
    {
        double _AverageValue = 0;
        double _StdDeviation = 0;

        bool _DisplayStats = false;

        int _DUTPinID;

        public bool Edited = false;
        bool LCLInitialized = false;
        bool UCLInitialized = false;

        public event EventHandler LimitsEdited;

        private void Limits_Edited(object sender, EventArgs e)
        {
            // invoke UserControl event here
            if (this.LimitsEdited != null) this.LimitsEdited(sender, e);
        }

        public void ChangeLimitsBySigmaAmount(int SigmaNumber)
        {
            Edited = true;
            Limits_Edited(null, null);
            LCL = AverageValue - (SigmaNumber * StandardDeviation);
            UCL = AverageValue + (SigmaNumber * StandardDeviation);
        }

        //public bool DisplayStats
        //{
        //    get
        //    {
        //        return _DisplayStats;
        //    }
        //    set
        //    {
        //        _DisplayStats = value;
        //        if (labelAverage.InvokeRequired == true)
        //        {
        //            this.Invoke(new MethodInvoker(delegate
        //            {
        //                labelAverage.Visible = _DisplayStats;
        //                labelStdDev.Visible = _DisplayStats;
        //            }));
        //        }
        //        else
        //        {
        //            labelAverage.Visible = _DisplayStats;
        //            labelStdDev.Visible = _DisplayStats;
        //        }

        //    }
        //}

        public int DutPinID
        {
            get { return _DUTPinID; }
            set
            {
                _DUTPinID = value;
              
                labelPin.Content = "DUT Pin " + _DUTPinID.ToString();
                   
            }
        }

        public double StandardDeviation
        {
            get
            {
                return _StdDeviation;
            }
            set
            {
                _StdDeviation = value;
             
                //labelStdDev.Text = "Std Dev [V]: " + _StdDeviation.ToString();
            }
        }

        public double AverageValue
        {
            get
            {
                return _AverageValue;
            }
            set
            {
                _AverageValue = value;
               
                
                    //labelAverage.Text = "Average [V]: " + _AverageValue.ToString();
            }
        }

        public double LCL
        {
            get
            {
                return numericLCL.pinValue();
            }
            set
            {
                double temp = value;
                if (temp < 0)
                    temp = 0;
        
                numericLCL.setValue(temp);

                if (LCLInitialized == true)
                {
                    Limits_Edited(null, null);
                    Edited = true;
                }
                LCLInitialized = true;
            }
        }

        public double UCL
        {
            get
            {
                return (double)numericUCL.pinValue();
            }
            set
            {
                double temp = value;
                if (temp < 0)
                    temp = 0;
            
                numericUCL.setValue(temp);
                if (UCLInitialized == true)
                {
                    Limits_Edited(null, null);
                    Edited = true;
                }
                UCLInitialized = true;
            }
        }

        public Limits()
        {
            InitializeComponent();
        }

        private void Limit_Loaded(object sender, EventArgs e)
        {
            numericLCL.ValueChanged += new EventHandler(numericLCL_ValueChanged);
            numericUCL.ValueChanged += new EventHandler(numericUCL_ValueChanged);
        }

        private void numericLCL_ValueChanged(object sender, EventArgs e)
        {
            if (LCLInitialized == true)
            {
                Limits_Edited(sender, e);
                Edited = true;
            }
            LCLInitialized = true;
        }

        private void numericUCL_ValueChanged(object sender, EventArgs e)
        {
            if (UCLInitialized == true)
            {
                Limits_Edited(sender, e);
                Edited = true;
            }
            UCLInitialized = true;
        }


    }
}
