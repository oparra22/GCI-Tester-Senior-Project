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
    /// doubleeraction logic for numericUpDown.xaml
    /// </summary>
    public partial class DoubleNumericUpDown : UserControl
    {

        double pinNum;
        double lowerLimit = 0;
        string pinString;
        //double clicked = 0;
        //sets default value of numeric up and down
        public void setValue(double v)
        {
            pinNum = v;
            pinString = pinNum.ToString();
            counterBox.Text = pinString;
        }

        //sets the lower limit 
        public void setLimit(double l)
        {
            lowerLimit = l;
        }

        public DoubleNumericUpDown()
        {
            InitializeComponent();
        }

        private void upPinButton_Click(object sender, RoutedEventArgs e)
        {
            pinNum++;
            pinString = pinNum.ToString();
            counterBox.Text = pinString;
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
        }

        public double pinValue()
        {
            return pinNum;
        }

       
    }
}
