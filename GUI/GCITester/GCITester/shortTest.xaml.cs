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
        private int FuckSeth = 0;
        private int resultsIndex = 0;
        int flag;
        // public ObservableCollection<int> names = new ObservableCollection<int>();




        public shortTest()
        {

            InitializeComponent();
            // List<int> listPins = new List<int>();
            //   PossiblePins.ItemsSource = listPins;
            //  ICollectionView view = CollectionViewSource.GetDefaultView(PossiblePins.ItemsSource);

        }



        //when test pins is clicked
        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            //int PinIn = Convert.ToInt32(pinIn.pinValue());
            int pin1 = Convert.ToInt32(shortTestControl.pinValue());
            int pin2 = Convert.ToInt32(shortTestControl.PossiblePins.SelectedItem);
            Console.WriteLine("Second pin selected from dropdown menu = " + pin2);
            Byte Pin1_1;
            Byte Pin1_2;
            Byte Pin2_1;
            Byte Pin2_2;

            if (pin1 > 254)
            {
                Pin1_1 = (Byte)254;
                Pin1_2 = (Byte)(pin1 - 254);
            }
            else
            {
                Pin1_1 = (Byte)pin1;
                Pin1_2 = (Byte)0;
            }
            if (pin2 > 254)
            {
                Pin2_1 = (Byte)254;
                Pin2_2 = (Byte)(pin2 - 254);
            }
            else
            {
                Pin2_1 = (Byte)pin2;
                Pin2_2 = (Byte)0;
            }
            //Communication.TestPinShort(Pin1_1, Pin1_2, Pin2_1, Pin2_2);
        }

        private void PossiblePins_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //List<string> data = new List<string>();   
            var comboBox = sender as ComboBox;
        }


        private void PossiblePins_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            //  PossiblePins_Loaded(sender, e);
            //  CollectionViewSource.GetDefaultView(PossiblePins.ItemsSource).Refresh();
        }

        private void pinIn_Loaded_1(object sender, RoutedEventArgs e)
        {
            //  CollectionViewSource.GetDefaultView(PossiblePins.ItemsSource).Refresh();

            // CollectionViewSource.GetDefaultView(PossiblePins.ItemsSource).Refresh();
            //PossiblePins.Items.Clear();
            //PossiblePins_Loaded(sender, e);
            //Console.WriteLine("Fuck Seth the whore");
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
                AddLog("Passed. Pins Tested: Pin1:" + TestedPin + " Pin2:" + TestedPin2 + ". Resistance:"  + Resistance + "Ohms." + ". VoltageIn read:" + VoltageIn + " VoltageDrop:" + Voltagedrop);

            }
            else
            {
                shortPass = false;
                AddLog("Failed. Pins Tested: Pin1:" + TestedPin + " Pin2:" + TestedPin2 + ". Resistance:" + Resistance + "Ohms." + ". VoltageIn read:" + VoltageIn + " VoltageDrop:" + Voltagedrop);
            }
            Communication.SetResultLED(shortPass);
            //AddLog("Pin " + TestedPin.ToString() + " Measured: " + VoltageIn.ToString() + "V  [0x" + Communication.PinValue.ToString("X4") + "]" + "\t Voltage Drop: " + (double)(VoltageRef - Voltage) + "Testedpin 2 = " + TestedPin2);
            AddLog("Pin1:" + TestedPin +" Pin2:" + TestedPin2 + ". VoltageIn read:" + VoltageIn + " VoltageDrop:" +Voltagedrop+ ". Resistance:" + Resistance+ "Ohms.");
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
    }

}

