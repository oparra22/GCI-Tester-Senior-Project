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

        int pinNum = 0;
        string pinString;

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
            if (pinNum != 0)
            {
                pinNum--;
            }
            pinString = pinNum.ToString();
            counterBox.Text = pinString;
        }

    }
}
