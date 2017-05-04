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
        string SelectedTestType = string.Empty;

        Dictionary<int, int> GCIToDUTMap = new Dictionary<int, int>();
        Dictionary<int, LearnResult> LearnResults;
        List<LearnControl> LearnControl;
        int CurrentDeviceNumber = 0;
        int TotalDevices = 0;
        public productionLimits()
        {
            InitializeComponent();
            numPartsTestCount.setValue(1);
            numPartsTestCount.setLimit(1);
            numIterPartCount.setValue(1);
            numIterPartCount.setLimit(1);
        }

        private void updownTest_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //Button start copied and complete
        private void start_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTestType == "Continuity")
            {
                //Selected Test is Continuous
                LearnResults = new Dictionary<int, LearnResult>();
                LearnControl = new List<LearnControl>();
                List<int> PinsToTest = GCIToDUTMap.Keys.ToList<int>();
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

                int NextPin = LearnControl[CurrentDeviceNumber].GetNextPin();

                SetButtonState(false);
                //This file is commented out but need to be uncommented in order to test the pin
                if (NextPin != 0)
                    Communication.TestPin(NextPin);
            }
            else if(SelectedTestType == "Short")
            {
                //Short test selected
                //Selected Test is Continuous
                LearnResults = new Dictionary<int, LearnResult>();
                LearnControl = new List<LearnControl>();
                List<int> PinsToTest = GCIToDUTMap.Keys.ToList<int>();

                CurrentDeviceNumber = 0;

                TotalDevices = (int)numPartsTestCount.pinValue();//Number is called pin value, will need to be adjusted to just value
                int TotalIterations = (int)numIterPartCount.pinValue();

                LearnControl ControlSetup = new LearnControl();
                ControlSetup.PinsToTest = PinsToTest;
                ControlSetup.TotalIterations = TotalIterations;
                //need to return the pin tested and the pins tested against
                for (int i = 0; i < TotalDevices; i++)
                {
                    LearnControl.Add(ControlSetup);
                }

                int NextPin = LearnControl[CurrentDeviceNumber].GetNextPin();

                SetButtonState(false);
                //This file is commented out but need to be uncommented in order to test the pin
                if (NextPin != 0)
                {
                    List<int> listPins = new List<int>();
                    listPins = GetPossibleValues(NextPin);  //Returns the list of possible pins that pin can test against
                    for(int s = 0; s < listPins.Count(); s++)
                    {
                        Communication.TestPinShort(NextPin, listPins[s]);
                    }
                }
                    
            }
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
                Console.WriteLine("Fuck Baldy");
                for (int i = channelPins; i < channelPins + 32; i++)
                {
                    even1 = i % 2;
                    //if pin is odd, add to list of possible pins
                    if (even1 == 0)
                    {
                        returnList.Add(i);

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

                        returnList.Add(i);
                        Console.WriteLine("Fuck Bob");
                        //PossiblePins.Items.Add(i);

                    }
                }
            }
            return returnList;
        }

        private void cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PopulatePartList()
        {
            partNumberBox.Items.Clear();
            //Add if statement, if  continous or short
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
            int TestedPin = Communication.PinID1 + Communication.PinID2;
            int TestedPin2 = Communication.PinID3 + Communication.PinID4;
            Double VoltageIn = (Communication.PinValue * VoltageRef) / 1023.0;
            Double Voltagedrop = (VoltageRef - VoltageIn);
            Double Current = Voltagedrop / 4700;
            Double Resistance = VoltageIn / Current;
            if (LearnResults.ContainsKey(TestedPin))
            {
                LearnResults[TestedPin].VoltageReadings.Add(VoltageIn);
            }
            else
            {
                LearnResults.Add(TestedPin, new LearnResult(TestedPin, VoltageIn));
            }


            if (CurrentDeviceNumber < LearnControl.Count)
            {
                int NextPin = LearnControl[CurrentDeviceNumber].GetNextPin();
                UpdateCurrentIteration(true);
                if (NextPin != 0)
                {
                    //this item was already commented out, leave it commented
                    //UpdateCurrentIteration(true); 
                    //This code has not been tested, continuity works but probably not short.
                    if (SelectedTestType == "Continuity")
                    {
                        Communication.TestPin(NextPin);
                    }
                    else if(SelectedTestType == "Short")
                    {
                        List<int> listPins = new List<int>();
                        listPins = GetPossibleValues(NextPin);  //Returns the list of possible pins that pin can test against
                        for (int s = 0; s < listPins.Count(); s++)
                        {
                            Communication.TestPinShort(NextPin, listPins[s]);
                        }
                    }

                    
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

                foreach (int PinID in LearnResults.Keys)
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
                try { labelProgress.Content = "Progress (" + (CurrentDeviceNumber + 1).ToString() + " / " + TotalDevices.ToString() + ")  Pin (" + (LearnControl[CurrentDeviceNumber].CurrentPinNumber + 1).ToString() + "/" + LearnControl[CurrentDeviceNumber].PinsToTest.Count.ToString() + ")"; }
                catch { Console.WriteLine("catch ran after progress test finish"); }
                
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
                foreach (int PinID in LearnResults.Keys)
                {
                    int DutPinID = GCIToDUTMap[PinID];

                    Double Average = LearnResults[PinID].GetVoltageAverage();
                    Double StdDev = LearnResults[PinID].GetStandardDeviation();

                    int NumberOfSigmas = (int)numericSigma.pinValue();

                    Double UCL = Average + NumberOfSigmas * StdDev;
                    Double LCL = Average - NumberOfSigmas * StdDev;

                    GCIDB.AddProductionLimit(ProductionLimitID, DutPinID, UCL, LCL, Average, StdDev);
                }

                MessageBox.Show("Limits saved to the database", "Success", MessageBoxButton.OK);
            }

          //  this.DialogResult = this.ShowDialog();
       //     this.Close();
        }

        private void productionLimits_Load(object sender, RoutedEventArgs e)
        {
            /****** All three of the next few lines will need to be uncommented, and the pin will be set as a getter/setter******/
            //numPartsTestCount.pinValue = Properties.Settings.Default.Learn_DefaultIterations;
            //numIterPartCount.pinValue = Properties.Settings.Default.Learn_DefaultNumberOfParts;
            //numericSigma.pinValue = Properties.Settings.Default.ProductionLimit_DefaultSigma;
            GCIDB.Initialize();
            testType_ComboBox.SelectedIndex = 0;
            SelectedTestType = "Continuous";
            //PopulatePartList(); // This may need to be changed to a different method, when the test is selected
            Communication.OnResultComplete += new Communication.ResultComplete(Communication_OnResultComplete);

        }

        private void testType_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox test = (ComboBox)sender;
            SelectedTestType = test.SelectedItem.ToString();
            PopulatePartList();//Repopulate the list when the testType combo box is changed
            Console.WriteLine($"Test Type Selected: {SelectedTestType}");
        }

        private void testType_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> testList = new List<string>();
            testList.Add("Continuity");
            testList.Add("Short");

            ComboBox test = (ComboBox)sender;
            test.ItemsSource =  testList;
        }
    }
}
