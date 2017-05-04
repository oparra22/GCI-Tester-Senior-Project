using System;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GCITester
{
    /// <summary>
    /// Interaction logic for shortTest.xaml
    /// </summary>


    public partial class shortTest : Window
    {

        //public List<int> listOfPins { get; set; }
        public List<int> listPins = new List<int>();
        private int resultsIndex = 0;
        int flag;



        public shortTest()
        {

            InitializeComponent();
            iterCount2.setValue(1);
            // List<int> listPins = new List<int>();
            //   PossiblePins.ItemsSource = listPins;
            //  ICollectionView view = CollectionViewSource.GetDefaultView(PossiblePins.ItemsSource);

        }



        //when test pins is clicked
        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            //get number of iterations
            int iter = iterCount2.pinValue();
            //int PinIn = Convert.ToInt32(pinIn.pinValue());
            int pin1 = Convert.ToInt32(shortTestControl.pinValue());
            int pin2 = Convert.ToInt32(shortTestControl.PossiblePins.SelectedItem);
            Console.WriteLine("Second pin selected from dropdown menu = " + pin2);

            //run the tests based on number of iterations
            for (int i = 0; i < iter; i++)
            {
                flag = 0;
                Communication.TestPinShort(pin1, pin2);
                while (true)
                {
                    if (flag == 1)
                    {
                        break;
                    }
                }
            }


        }



        private void pinIn_Loaded_1(object sender, RoutedEventArgs e)
        {
            //  CollectionViewSource.GetDefaultView(PossiblePins.ItemsSource).Refresh();

            // CollectionViewSource.GetDefaultView(PossiblePins.ItemsSource).Refresh();
            //PossiblePins.Items.Clear();
            //PossiblePins_Loaded(sender, e);
            Console.WriteLine("Fuck Seth the whore");
        }

        private void form1_load(object sender, RoutedEventArgs e)
        {

        }

        //added for log
        void Communication_OnResultComplete()
        {
            Double VoltageRef = Properties.Settings.Default.VoltageReference;
            int TestedPin = Communication.PinID1 + Communication.PinID2;
            int TestedPin2 = Communication.PinID3 + Communication.PinID4;
            Console.WriteLine($"Byte Pin1 = {Communication.PinID1} Byte Pin2 = {Communication.PinID2}");
            Double VoltageIn = Math.Round((Communication.PinValue * VoltageRef) / 1023.0, 3);
            Console.WriteLine("Onresultecomplete before AddLog");
            //values need to be changed for final PCB
            Double Voltagedrop = (VoltageRef - VoltageIn);
            Double Current = Voltagedrop / 4700;
            Double Resistance = VoltageIn / Current;
            bool shortPass = false;
            if (Resistance > 1000)
            {
                shortPass = true;
                AddLog("Passed. Pins Tested: " + TestedPin + " and " + TestedPin2 + ". Resistance:" + string.Format("{0:0.00}", Resistance) + "Ohms. VoltageDrop:" + Voltagedrop);

            }
            else
            {
                shortPass = false;
                AddLog("Failed. Pins Tested: Pin1: " + TestedPin + " Pin2: " + TestedPin2 + ". Resistance:" + Resistance + "Ohms. VoltageDrop:" + Voltagedrop);
            }
            Communication.SetResultLED(shortPass);
            //AddLog("Pin " + TestedPin.ToString() + " Measured: " + VoltageIn.ToString() + "V  [0x" + Communication.PinValue.ToString("X4") + "]" + "\t Voltage Drop: " + (double)(VoltageRef - Voltage) + "Testedpin 2 = " + TestedPin2);
            //AddLog("Pin1:" + TestedPin +" Pin2:" + TestedPin2 + ". VoltageIn read:" + VoltageIn + " VoltageDrop:" +Voltagedrop+ ". Resistance:" + Resistance+ "Ohms.");
            //AddLog("Bool shortPass = " + shortPass);
            //manualShortTestResults.ScrollIntoView(manualShortTestResults.Items[manualShortTestResults.Items.Count - 1]);
            //manualShortTestResults.ScrollIntoView(manualShortTestResults.SelectedItem);
            flag = 1;
        }


        private void AddLog(String Text)
        {
            //This block is the windows FORM version of the invoke method, as seen in GCI's code, 
            //this.Invoke(new MethodInvoker(delegate
            //{
            //    manualTestPinResults.Items.Add(Text);
            //}));

            //The adjusted invoke method needed to prevent a single thread being requested by multiple processes.
            //It does the same as the block commented out above, just converted for WPF
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                //resultsIndex++;
                manualShortTestResults.Items.Add(Text);
                manualShortTestResults.Items.MoveCurrentToLast();
                //manualShortTestResults.ScrollIntoView(manualShortTestResults.Items.CurrentItem);
                //manualShortTestResults.SelectedIndex = manualShortTestResults.Items.Count - 1;
                //manualShortTestResults.ScrollIntoView(manualShortTestResults.SelectedItem);
                manualShortTestResults.ScrollIntoView(manualShortTestResults.Items[manualShortTestResults.Items.Count - 1]);
            }));


        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Communication.OnResultComplete -= new Communication.ResultComplete(Communication_OnResultComplete);
        }



        private void manualShortTestResults_Loaded(object sender, RoutedEventArgs e)
        {
            Communication.OnResultComplete += new Communication.ResultComplete(Communication_OnResultComplete);
            manualShortTestResults.Items.Add("Fuck Seth");
            manualShortTestResults.Items.Add("Fuck baldy");
            manualShortTestResults.Items.Add("Fuck Bob");
            manualShortTestResults.Items.Add("Fuck Robert");
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            manualShortTestResults.Items.Clear();
        }

        private void shortTestControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void testAllButton_Click(object sender, RoutedEventArgs e)
        {
            //get number of iterations
            int iter = iterCount2.pinValue();
            //get list of possible pins
            List<int> listPins = new List<int>();
            //get pin 
            for (int pin = 1; pin < 10; pin++)
            {
                listPins = GetPossibleValues(pin);
                // Console.WriteLine("Test");

                for (int s = 0; s < listPins.Count(); s++)
                {

                    Communication.TestPinShort(pin, listPins[s]);
                    Console.WriteLine("Fucks that shit");
                    Console.WriteLine(listPins[s]);

                }

            }

            //List<int> PinsToTest = GCIToDUTMap.Keys.ToList<int>();
        }

        private List<int> GetPossibleValues(int PinIn)
        {
            List<int> returnList = new List<int>();
            int even1;
            int even2;
            int pair = (PinIn - 1) / 32;
            //check if odd or even. and return possible pins to test
            int odd = PinIn % 2;
            int channelPins = 32 * pair + 1;
            Console.WriteLine(odd);
            if (odd == 1)
            {
                //Console.WriteLine("Fuck Baldy");
                for (int i = channelPins; i < channelPins + 32; i++)
                {
                    even1 = i % 2;
                    //if pin is odd, add to list of possible pins
                    if (even1 == 0)
                    {
                        returnList.Add(i);

                        // Console.WriteLine("Fuck seth");
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

                        returnList.Add(i);
                        //Console.WriteLine("Fuck Bob");
                        //PossiblePins.Items.Add(i);

                    }
                }
            }
            return returnList;
        }
    }

}

