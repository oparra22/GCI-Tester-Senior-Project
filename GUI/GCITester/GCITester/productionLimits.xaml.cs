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
    /// Interaction logic for productionLimits.xaml
    /// This code will be mostly copied over from the frmLearn.cs file in the original code,
    /// translated for wpf
    /// </summary>
    public partial class productionLimits : Window
    {
        String SelectedPartName = String.Empty;
        String SelectedBoardName = String.Empty;
        string SelectedSocketName = string.Empty;

        Dictionary<Byte, Byte> GCIToDUTMap = new Dictionary<byte, byte>();
        Dictionary<Byte, LearnResult> LearnResults;
        List<LearnControl> LearnControl;
        int CurrentDeviceNumber = 0;
        int TotalDevices = 0;
        public productionLimits()
        {
            InitializeComponent();
        }






        private void updownTest_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //Button start copied and complete
        private void start_Click(object sender, RoutedEventArgs e)
        {
            LearnResults = new Dictionary<byte, LearnResult>();
            LearnControl = new List<LearnControl>();
            List<Byte> PinsToTest = GCIToDUTMap.Keys.ToList<Byte>();
            CurrentDeviceNumber = 0;
            TotalDevices = (int)numPartsTestCount.pinValue();//Number is called pin value, will need to be adjusted to just value
            int TotalIterations = (int)numIterPartCount.pinValue();

            LearnControl ControlSetup = new LearnControl();
            ControlSetup.PinsToTest = PinsToTest;
            ControlSetup.TotalIterations = TotalIterations;
            for (int i = 0; i < TotalDevices; i++)
            {
                LearnControl.Add(ControlSetup);
            }

            Byte NextPin = LearnControl[CurrentDeviceNumber].GetNextPin();

            SetButtonState(false);
            //This file is commented out but need to be uncommented in order to test the pin
            //if (NextPin != 0)
                //Communication.TestPin(NextPin);

        }

        private void cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PopulatePartList()
        {
            partNumberBox.Items.Clear();
            List<string> PartNames = GCIDB.GetPartList();
            foreach (string part in PartNames)
            {
                partNumberBox.Items.Add(part);
            }
        }
        private void PopulateBoardList()
        {
            List<String> BoardNames = GCIDB.GetTestBoardList(SelectedPartName);
            SelectedBoardName = string.Empty;
            testBoardBox.Text = string.Empty;
            testBoardBox.Items.Clear();
            foreach (String Board in BoardNames)
            {
                testBoardBox.Items.Add(Board);
            }
        }
        private void PopulateSocketList()
        {
            List<String> SocketNames = GCIDB.GetSocketList(SelectedPartName, SelectedBoardName);
            SelectedSocketName = string.Empty;
            socketOnBoardCombobox.Text = string.Empty;
            socketOnBoardCombobox.Items.Clear();
            foreach (String Socket in SocketNames)
            {
                socketOnBoardCombobox.Items.Add(Socket);
            }
        }
        
        private void partNumberBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (partNumberBox.Items.Count > 0)
            {
                if (partNumberBox.SelectedItem == null)
                    return;
                SelectedPartName = partNumberBox.SelectedItem.ToString();
                PopulateBoardList();

            }
        }
        private void testBoardBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (testBoardBox.Items.Count > 0)
            {
                if (testBoardBox.SelectedItem == null)
                    return;
                SelectedBoardName = testBoardBox.SelectedItem.ToString();
                PopulateSocketList();

            }
        }

        private void socketOnBoardCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (socketOnBoardCombobox.Items.Count > 0)
            {
                if (socketOnBoardCombobox.SelectedItem == null)
                    return;
                SelectedSocketName = socketOnBoardCombobox.SelectedItem.ToString();
                GCIToDUTMap = GCIDB.GetPinMap(SelectedPartName, SelectedBoardName, SelectedSocketName);
            }
        }
        void Communication_OnResultComplete()
        {
            Double VoltageRef = Properties.Settings.Default.VoltageReference;
            Byte TestedPin = (Byte)Communication.PinID1;  //Was PinID
            Double Voltage = (Communication.PinValue * VoltageRef) / 1023.0;

            if (LearnResults.ContainsKey(TestedPin))
            {
                LearnResults[TestedPin].VoltageReadings.Add(Voltage);
            }
            else
            {
                LearnResults.Add(TestedPin, new LearnResult(TestedPin, Voltage));
            }


            if (CurrentDeviceNumber < LearnControl.Count)
            {
                Byte NextPin = LearnControl[CurrentDeviceNumber].GetNextPin();
                UpdateCurrentIteration(true);
                if (NextPin != 0)
                {
                    //this item was already commented out, leave it commented
                    //UpdateCurrentIteration(true); 


                    //Communication.TestPin(NextPin); this line needs to be uncommented
                }
                else
                {
                    CurrentDeviceNumber++;
                    if (CurrentDeviceNumber != LearnControl.Count)
                    {
                        if (MessageBox.Show("Please place part #" + (CurrentDeviceNumber + 1).ToString() + " in the socket", "Next Part", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                        {
                            NextPin = LearnControl[CurrentDeviceNumber].GetNextPin();
                            //Communication.TestPin(NextPin); This needs to be uncommented
                        }
                    }
                    else
                    {
                        UpdateCurrentIteration(false);
                        SetButtonState(true);
                        BuildReport();
                        MessageBox.Show("Testing complete");
                    }

                }

            }

        }

        private void SetButtonState(bool Enabled)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                saveButton.IsEnabled = Enabled;
                startButton.IsEnabled = Enabled;
            }));
        }

        private void BuildReport()
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                treeResults.Items.Clear();//Changed treeResults.Nodes to treeResults.Nodes
                TreeViewItem Root = new TreeViewItem(); //TreeNode is now TreeViewItem
                //added line
                Root.Header = SelectedPartName;
                treeResults.Items.Add(Root);

                foreach (Byte PinID in LearnResults.Keys)
                {
                    TreeViewItem Name = new TreeViewItem();
                    Name.Header = "Pin " + GCIToDUTMap[PinID].ToString();

                    TreeViewItem Average = new TreeViewItem();
                    Average.Header = "Average: " + LearnResults[PinID].GetVoltageAverage() + " V";

                    TreeViewItem StdDev = new TreeViewItem();
                    StdDev.Header = "StdDev: " + LearnResults[PinID].GetStandardDeviation() + " V";

                    TreeViewItem MeasuredValues = new TreeViewItem();
                    MeasuredValues.Header = LearnResults[PinID].VoltageReadings.Count.ToString() + " Measured Voltages";
                    for (int i = 0; i < LearnResults[PinID].VoltageReadings.Count; i++)
                    {
                        TreeViewItem Value = new TreeViewItem();
                        Value.Header = "Iteration " + (i + 1).ToString() + ") " + LearnResults[PinID].VoltageReadings[i].ToString();
                        MeasuredValues.Items.Add(Value);
                    }
                    Name.Items.Add(Average);
                    Name.Items.Add(StdDev);
                    Name.Items.Add(MeasuredValues);
                    Root.Items.Add(Name);
                }
            }));
        }

        private void UpdateCurrentIteration(bool Visible)
        {
            
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                if (Visible == true)
                    labelProgress.Visibility = Visibility.Visible;
                else
                {
                    labelProgress.Visibility = Visibility.Hidden;
                    return;
                }
                labelProgress.Content = "Progress (" + (CurrentDeviceNumber + 1).ToString() + " / " + TotalDevices.ToString() + ")  Pin (" + (LearnControl[CurrentDeviceNumber].CurrentPinNumber + 1).ToString() + "/" + LearnControl[CurrentDeviceNumber].PinsToTest.Count.ToString() + ")";
            }));
        }

        private void productionLimits_OnClosing(object sender, CancelEventArgs e)
        {
            Communication.OnResultComplete -= new Communication.ResultComplete(Communication_OnResultComplete);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            int ProductionLimitID = GCIDB.AssociatePartToNewProductionLimit(SelectedPartName);
            if (ProductionLimitID != 0)
            {
                /*List<LimitEntity> UserLimits = BuildLimitEntityList();
                foreach (LimitEntity entry in UserLimits)
                {
                    GCIDB.AddProductionLimit(LimitID, entry.PinID, entry.UCL, entry.LCL);
                }*/
                foreach (Byte PinID in LearnResults.Keys)
                {
                    Byte DutPinID = GCIToDUTMap[PinID];

                    Double Average = LearnResults[PinID].GetVoltageAverage();
                    Double StdDev = LearnResults[PinID].GetStandardDeviation();

                    int NumberOfSigmas = (int)numericSigma.pinValue();

                    Double UCL = Average + NumberOfSigmas * StdDev;
                    Double LCL = Average - NumberOfSigmas * StdDev;

                    GCIDB.AddProductionLimit(ProductionLimitID, DutPinID, UCL, LCL, Average, StdDev);
                }

                MessageBox.Show("Limits saved to the database", "Success", MessageBoxButton.OK);
            }

            this.DialogResult = this.ShowDialog();
            this.Close();
        }

        private void productionLimits_Load(object sender, RoutedEventArgs e)
        {
            /****** All three of the next few lines will need to be uncommented, and the pin will be set as a getter/setter******/
            //numPartsTestCount.pinValue = Properties.Settings.Default.Learn_DefaultIterations;
            //numIterPartCount.pinValue = Properties.Settings.Default.Learn_DefaultNumberOfParts;
            //numericSigma.pinValue = Properties.Settings.Default.ProductionLimit_DefaultSigma;
            GCIDB.Initialize();
            PopulatePartList();
            Communication.OnResultComplete += new Communication.ResultComplete(Communication_OnResultComplete);

        }
    }
}
