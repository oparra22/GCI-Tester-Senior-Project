using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GCITester
{
    /// <summary>
    /// Interaction logic for pinTest.xaml
    /// </summary>
    public partial class pinTest : Window
    {
        int flag; 

        public pinTest()
        {
            InitializeComponent();
            //Here are we are setting initial values and lower limits for the counters (since it was user created)
            pinCount.setLimit(1);
            pinCount.setValue(1);
            iterCount.setValue(1);
            iterCount.setLimit(1);
        }


        private void testButton_Click(object sender, RoutedEventArgs e)
        {
           //Determine the number of iterations to run the manual test
            int iter = iterCount.pinValue();

            for (int i = 0; i < iter; i++)
            {
                flag = 0;
                testPin();
                while (true)
                {
                    if (flag == 1)
                    {
                        break;
                    }

                }
            }
        }

        //function sends data to communications file for pin testing
        void testPin()
        {

            //listBox.Items.Add("Pin " + "Measured");
            // listBox.SelectedIndex = listBox.Items.Count - 1;
            //Kommon Added Code
            //int PinIn = Convert.ToInt32(pinCount.Content);
            int currentPin = pinCount.pinValue();
            int PinIn = currentPin;
            //Had to change pinCount.Text to pinCount.Content, need to double check if it works
           
            //For debugging, comment out of final
          //  Console.WriteLine($"Information sent to Communication.cs from testPinButton_Click:\tPin to test = {PinIn}, Byte1 = {Pin1} Byte2 = {Pin2}");

            Communication.TestPin(PinIn);
        }


        void Communication_OnResultComplete()
        {
            Double VoltageRef = Properties.Settings.Default.VoltageReference;
            int TestedPin = Communication.PinID1 + Communication.PinID2;
            int TestedPin2 = Communication.PinID3 + Communication.PinID4;
            Console.WriteLine($"Byte Pin1 = {Communication.PinID1} Byte Pin2 = {Communication.PinID2}");
            Double Voltage = Math.Round((Communication.PinValue * VoltageRef) / 1023.0, 3);
            Console.WriteLine("Onresultecomplete before AddLog");
            //AddLog("Pin " + TestedPin.ToString() + " Measured: " + Voltage.ToString() + "V  [0x" + Communication.PinValue.ToString("X4") + "]" + "\t Voltage Drop: " + (double)(VoltageRef - Voltage) + "Testedpin 2 = " + TestedPin2);
            AddLog($"Pin:{TestedPin.ToString()} Measured: {Voltage.ToString()}V Drop: {(double)(VoltageRef - Voltage)} ");
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
                manualTestPinResults.Items.Add(Text);
                manualTestPinResults.Items.MoveCurrentToLast();
                manualTestPinResults.ScrollIntoView(manualTestPinResults.Items.CurrentItem);
            }));

        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Communication.OnResultComplete -= new Communication.ResultComplete(Communication_OnResultComplete);
        }

        private void manualTestPinResults_Loaded(object sender, RoutedEventArgs e)
        {
            Communication.OnResultComplete += new Communication.ResultComplete(Communication_OnResultComplete);
        }
    }
}
