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
    /// Interaction logic for numericUpDown.xaml
    /// </summary>
    public partial class numericUpDown : UserControl
    {

        int pinNum;
        int lowerLimit = 0;
        string pinString;
        //int clicked = 0;
        //sets default value of numeric up and down
        public void setValue(int v)
        {
            pinNum = v;
            pinString = pinNum.ToString();
            counterBox.Text = pinString;
        }

        //sets the lower limit 
        public void setLimit(int l)
        {
            lowerLimit = l;
        }

        public numericUpDown()
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

        public int pinValue()
        {
            return pinNum;
        }

        private void numericUpDown_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            pinNum = Convert.ToInt32(t.Text);
        }
    }
}
