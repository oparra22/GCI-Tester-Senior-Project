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
using System.Windows.Shapes;
//using System.Windows.Mess


namespace GCITester
{
    /// <summary>
    /// Interaction logic for productionTest.xaml
    /// </summary>
    public partial class productionTest : Window
    {

        String SelectedPartName = String.Empty;
        String SelectedBoardName = String.Empty;
        int LoadedTestBoardID = 0;
        int LoadedPartID = 0;
        int LoadedProductionLimitID = 0;
        List<TestPinEntity> LoadedTestInfo = new List<TestPinEntity>();
        List<LimitEntity> ProductionPartLimits = new List<LimitEntity>();
        Dictionary<int, int> GCItoDUTMap = new Dictionary<int, int>();
        Dictionary<int, int> GCItoDeviceIndex = new Dictionary<int, int>();
        Dictionary<int, LimitEntity> DUTPintoLimit = new Dictionary<int, LimitEntity>();

        List<int> PinsToTest = new List<int>();
        //int CurrentPinNumber = 0;
        int PinTotal = 0;

        TestPart ProductionTest = new TestPart();

        public productionTest()
        {
            InitializeComponent();
        }

        private void listProductionInfo1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateStartButtonState()
        {
            if (SelectedPartName.Length > 0 && SelectedBoardName.Length > 0 && textBatchName.Text.Length > 0)
            {
                SetDisplayStates(true);
            }
        }

        private void SetDisplayStates(bool State)
        {

            //this.Invoke(new MethodInvoker(delegate
            //{
            //    buttonStart.Enabled = State;
            //}));
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                buttonStart.IsEnabled = State;
            }));
            

        }

        private void UpdateCurrentIteration(bool Visible)
        {
            if (labelProgress.Dispatcher.CheckAccess() == true)
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
                    labelProgress.Content = "Iteration (" + (ProductionTest.CurrentIteration + 1).ToString() + "/" + ProductionTest.TotalIterations.ToString() + ") Pin (" + (ProductionTest.CurrentPinNumber + 1).ToString() + "/" + PinTotal.ToString() + ")";
                }));
            }
            else
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
                    labelProgress.Content = "Iteration (" + (ProductionTest.CurrentIteration + 1).ToString() + "/" + ProductionTest.TotalIterations.ToString() + ") Pin (" + (ProductionTest.CurrentPinNumber + 1).ToString() + "/" + PinTotal.ToString() + ")";
                }));
               }
        }

        private void PopulatePartList()
        {
            comboPartName.Items.Clear();
            List<string> PartNames = GCIDB.GetPartList();
            foreach (string part in PartNames)
            {
                comboPartName.Items.Add(part);
            }
        }

        private void PopulateBoardList()
        {
            List<String> BoardNames = GCIDB.GetTestBoardList(SelectedPartName);
            SelectedBoardName = string.Empty;
            comboTestBoard.Text = string.Empty;
            comboTestBoard.Items.Clear();
            foreach (String Board in BoardNames)
            {
                comboTestBoard.Items.Add(Board);
            }
            
        }

        private void productionTest_Load(object sender, RoutedEventArgs e)
        {
            GCIDB.Initialize();
            PopulatePartList();
            numericIterations.setValue(Properties.Settings.Default.Production_DefaultIterations);
            Communication.OnResultComplete += new Communication.ResultComplete(Communication_OnResultComplete);
        }

        void Communication_OnResultComplete()
        {
            Double VoltageRef = Properties.Settings.Default.VoltageReference;
            int TestedPin = Communication.PinID1 + Communication.PinID2;
            Double Voltage = Math.Round((Communication.PinValue * VoltageRef) / 1023.0, 3);

            int BelongsToSocketId = GCItoDeviceIndex[TestedPin];
            int DUTPin = GCItoDUTMap[TestedPin];
            //LimitEntity LimitsForPin = DUTPintoLimit[DUTPin];

            bool PinResult = false;

            ProductionTest.AddResult(BelongsToSocketId, DUTPin, Voltage);
            int NextPin = ProductionTest.GetNextPin();
            if (NextPin != 0)
            {
                UpdateCurrentIteration(true);
                Communication.TestPin(NextPin);
            }
            else
            {
                foreach (int Socket in ProductionTest.TestResults.Keys)
                {
                    int ProductionTestID = GCIDB.GetNextProductionTestID();
                    foreach (int TestedDUTPin in ProductionTest.TestResults[Socket].Keys)
                    {
                        LimitEntity LimitsForPin = DUTPintoLimit[TestedDUTPin];

                        double AverageVoltage = ProductionTest.TestResults[Socket][TestedDUTPin].GetVoltageAverage();
                        double StdDev = ProductionTest.TestResults[Socket][TestedDUTPin].GetStandardDeviation();

                        if (AverageVoltage < LimitsForPin.LCL || AverageVoltage > LimitsForPin.UCL)
                        {
                            PinResult = false;
                            listProductionInfo1.SetResult(Socket, false);
                            AddLog("Socket " + Socket + " DUT Pin [" + TestedDUTPin + "] Average Voltage: " + AverageVoltage + " FAILED");
                        }
                        else
                        {
                            PinResult = true;
                            AddLog("Socket " + Socket + " DUT Pin [" + TestedDUTPin + "] Average Voltage: " + AverageVoltage + " PASSED");
                        }

                        for (int j = 0; j < ProductionTest.TestResults[Socket][TestedDUTPin].VoltageReadings.Count; j++)
                        {
                            
                            double MeasuredVoltage = ProductionTest.TestResults[Socket][TestedDUTPin].VoltageReadings[j];
                            Dispatcher.BeginInvoke(new Action(delegate ()
                            {
                               
             
                                GCIDB.AddProductionTestData(textBatchName.Text, ProductionTestID, LoadedPartID, LoadedProductionLimitID, TestedDUTPin, (j-1), MeasuredVoltage, AverageVoltage, StdDev, PinResult, DateTime.Now);
                                
                            }));
                            }
                    }
                }
                SetDisplayStates(true);
                UpdateCurrentIteration(false);
                listProductionInfo1.DisplaAll(true);
             //  MessageBox.Show("Test Complete", "Part Finished", MessageBoxButton.OK, MessageBox.Information);
            }


        }

        private void AddLog(string Text)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                listLog.Items.Add(Text);
            }));
            

        }

        private void ClearLog()
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                listLog.Items.Clear();
            }));
            

        }

        private void comboPartName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPartName.Items.Count > 0)
            {
                if (comboPartName.SelectedItem == null)
                    return;
                SelectedPartName = comboPartName.SelectedItem.ToString();
                PopulateBoardList();
                listProductionInfo1.Clear();
                ProductionPartLimits = GCIDB.GetProductionLimits(SelectedPartName);
                if (ProductionPartLimits.Count > 0)
                {
                    LoadedPartID = ProductionPartLimits[0].PartID;
                    LoadedProductionLimitID = ProductionPartLimits[0].ProductionLimitID;
                    DUTPintoLimit = new Dictionary<int, LimitEntity>();

                    foreach (LimitEntity Limit in ProductionPartLimits)
                    {
                        if (DUTPintoLimit.ContainsKey(Limit.PinID) == false)
                        {
                            DUTPintoLimit.Add(Limit.PinID, Limit);
                        }
                    }
                }
                UpdateStartButtonState();
            }
        }

        private void frmProductionTest_FormClosing(object sender, EventArgs e)
        {
            Communication.OnResultComplete -= new Communication.ResultComplete(Communication_OnResultComplete);
        }

        private void comboTestBoard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTestBoard.Items.Count > 0)
            {
                if (comboTestBoard.SelectedItem == null)
                    return;
                SelectedBoardName = comboTestBoard.SelectedItem.ToString();
                LoadTestInformation();
               UpdateStartButtonState();
            }
        }

        List<int> GetGCITestPinsFromIndex(int Index)
        {
            List<int> Result = new List<int>();
            if (LoadedTestInfo.Count > 0)
            {
                foreach (TestPinEntity Pin in LoadedTestInfo)
                {
                    if (Pin.SocketIndex == Index)
                    {
                        Result.Add(Pin.GCIPin);
                    }
                }

            }
            return Result;
        }

        void LoadTestInformation()
        {
            LoadedTestInfo = GCIDB.GetTestPins(SelectedPartName, SelectedBoardName);
            GCItoDUTMap = new Dictionary<int, int>();
            GCItoDeviceIndex = new Dictionary<int, int>();

            if (LoadedTestInfo.Count > 0)
            {
                LoadedTestBoardID = LoadedTestInfo[0].TestBoardID;
            }

            List<string> SlotNames = new List<string>();
            List<int> SocketIndex = new List<int>();
            foreach (TestPinEntity Pin in LoadedTestInfo)
            {
                if (SlotNames.Contains(Pin.SocketName) == false)
                {
                    SlotNames.Add(Pin.SocketName);
                    SocketIndex.Add(Pin.SocketIndex);
                }

                if (GCItoDUTMap.ContainsKey(Pin.GCIPin) == false)
                {
                    GCItoDUTMap.Add(Pin.GCIPin, Pin.DUTPin);
                }

                if (GCItoDeviceIndex.ContainsKey(Pin.GCIPin) == false)
                {
                    GCItoDeviceIndex.Add(Pin.GCIPin, Pin.SocketIndex);
                }
            }
            listProductionInfo1.Clear();

            for (int i = 0; i < SlotNames.Count; i++)
            {
                String Slot = SlotNames[i];
                int Index = SocketIndex[i];
                List<int> GCIPinsToTest = GetGCITestPinsFromIndex(Index);
                listProductionInfo1.AddProductionTestSlot(Slot, Index, LoadedTestBoardID, GCIPinsToTest);
            }
            
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            PinsToTest = listProductionInfo1.GetAllPinsToTest();
            listProductionInfo1.ResetAll();
            PinTotal = PinsToTest.Count;
            ClearLog();

            ProductionTest = new TestPart();
            ProductionTest.TotalIterations = (int)numericIterations.pinValue();
            ProductionTest.PinsToTest = PinsToTest;

            if (PinTotal > 0)
            {
                SetDisplayStates(false);
                Communication.TestPin(ProductionTest.GetNextPin());
            }
            /*  Debuggin purposes
            listProductionInfo1.DisplaAll(true);
            listProductionInfo1.SetResult(0, false);
            */
        }

        private void labelProgress_Click(object sender, EventArgs e)
        {

        }

        private void textBatchName_TextChanged(object sender, EventArgs e)
        {
            UpdateStartButtonState();
        }

       
    }
}



