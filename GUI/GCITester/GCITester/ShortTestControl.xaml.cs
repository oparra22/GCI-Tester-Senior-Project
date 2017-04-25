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
    /// Interaction logic for ShortTestControl.xaml
    /// </summary>
    public partial class ShortTestControl : UserControl
    {
        int pinNum = 1;
        int lowerLimit = 1;
        string pinString;

        public void setValue(int v)
        {
            pinNum = v;
            pinString = pinNum.ToString();
            counterBox_Short.Text = pinString;
        }

        public void setLimit(int l)
        {
            lowerLimit = l;
        }

        public ShortTestControl()
        {
            InitializeComponent();
        }

        private void upPinButton_Click(object sender, RoutedEventArgs e)
        {
            pinNum++;
            pinString = pinNum.ToString();
            counterBox_Short.Text = pinString;
            PossiblePins_Loaded(sender, e);

        }
        //decrement pin number
        private void downPinButton_Click(object sender, RoutedEventArgs e)
        {
            if (pinNum != lowerLimit)
            {
                pinNum--;
            }
            pinString = pinNum.ToString();
            counterBox_Short.Text = pinString;
            PossiblePins_Loaded(sender, e);

        }

        public int pinValue()
        {
            return pinNum;
        }

        private void PossiblePins_Loaded(object sender, RoutedEventArgs e)
        {
            int PinIn = Convert.ToInt32(pinNum);

            List<int> listPins = new List<int>();
            int even1;
            int even2;

            var comboBox = sender as ComboBox;

            //check which channel pin is on
            int pair = (PinIn - 1) / 32;
            //check if odd or even. and return possible pins to test
            int odd = PinIn % 2;
            int channelPins = 32 * pair + 1;
            Console.WriteLine(odd);
            if (odd == 1)
            {
                Console.WriteLine("Fuck Baldy");
                for (int i = channelPins; i < channelPins + 32; i++)
                {
                    even1 = i % 2;
                    //if pin is odd, add to list of possible pins
                    if (even1 == 0)
                    {
                        listPins.Add(i);

                        Console.WriteLine("Fuck seth");
                        //PossiblePins.Items.Add(i);
                    }
                }
            }
            else if (odd == 0)
            {
                for (int i = channelPins; i < channelPins + 32; i++)
                {
                    even2 = i % 2;
                    //if pin is even, add to list possible pins
                    if (even2 == 1)
                    {

                        listPins.Add(i);
                        Console.WriteLine("Fuck Bob");
                        //PossiblePins.Items.Add(i);

                    }
                }
            }

            PossiblePins.ItemsSource = listPins;
            PossiblePins.SelectedIndex = 0;
        }

        private void counterBox_Short_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                pinNum = Convert.ToInt32(tb.Text);
            }
            catch (Exception k)
            {
                Console.WriteLine("Error: Exception - " + k.Message);
            }
        }


    }
}
