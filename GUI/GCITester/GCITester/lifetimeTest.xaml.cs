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

namespace GCITester
{
    /// <summary>
    /// Interaction logic for lifetimeTest.xaml
    /// </summary>
    public partial class lifetimeTest : Window
    {
        String SelectedPartName = String.Empty;
        String SelectedBoardName = String.Empty;
        int LoadedTestBoardID = 0;
        int LoadedPartID = 0;
        int LoadedLifetimeLimitID = 0;
        List<TestPinEntity> LoadedTestInfo = new List<TestPinEntity>();
        LifetimeLimitEntity LifetimePartLimits = new LifetimeLimitEntity();
        Dictionary<int, int> GCItoDUTMap = new Dictionary<int, int>();
        Dictionary<int, int> GCItoDeviceIndex = new Dictionary<int, int>();
        //Dictionary<int, LimitEntity> DUTPintoLimit = new Dictionary<int, LimitEntity>();
        List<String> ExistingSerialNumbers = new List<string>();
        List<int> PinsToTest = new List<int>();

        TestPart LifetimeTest = new TestPart();

        public lifetimeTest()
        {
            InitializeComponent();
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

        private void LifetimeTest_Load(object sender, EventArgs e)
        {
            GCIDB.Initialize();
            PopulatePartList();
            Communication.OnResultComplete += new Communication.ResultComplete(Communication_OnResultComplete);
            numericIterations.setValue(Properties.Settings.Default.Lifetime_DefaultIterations);
            numericTemperature.setValue((int)Properties.Settings.Default.Lifetime_DefaultTemperature);
        }

        void SaveDataToDatabase()
        {
            try
            {

                foreach (int Socket in LifetimeTest.TestResults.Keys)
                {
                    string CurrentSerialNumber = listLifetimeInfo1.GetSerialNumber(Socket);
                    if (numericTestHour.pinValue() == 0)
                    {
                        int BaselineID = GCIDB.GetMostRecentLifetimeTestID_BaseLine(LoadedPartID, CurrentSerialNumber, textBatchName.Text.Trim());
                        //Look at this later
                        //if (BaselineID != 0)
                        //{
                        //    if (MessageBox.Show("Baseline data already exists for:\nPart: " + SelectedPartName + "\nSerial Number: " + CurrentSerialNumber + "\n\nDo you want to change the baseline data to these measurements?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                        //    {
                        //        continue;
                        //    }
                        //    else
                        //        return;

                        //}
                    }

                    int LifetimeTestID = GCIDB.GetNextLifetimeTestID();

                    String Batch = textBatchName.Text;
                    DateTime Time = DateTime.Now;
                    int Hour = (int)numericTestHour.pinValue();
                    double Temperature = (double)numericTemperature.pinValue();

                    foreach (int TestedDUTPin in LifetimeTest.TestResults[Socket].Keys)
                    {
                        double AverageVoltage = LifetimeTest.TestResults[Socket][TestedDUTPin].GetVoltageAverage();
                        double StdDev = LifetimeTest.TestResults[Socket][TestedDUTPin].GetStandardDeviation();

                        for (int j = 0; j < LifetimeTest.TestResults[Socket][TestedDUTPin].VoltageReadings.Count; j++)
                        {
                            double MeasuredVoltage = LifetimeTest.TestResults[Socket][TestedDUTPin].VoltageReadings[j];
                            GCIDB.AddLifetimeTestData(LifetimeTestID, CurrentSerialNumber, Batch, LoadedPartID, Hour, LoadedLifetimeLimitID, Temperature, TestedDUTPin, j, MeasuredVoltage, AverageVoltage, StdDev, Time);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SaveDataToDatabase(): " + ex.ToString());
            }
        }

        void DisplayTime0Data()
        {
            //this.Invoke(new MethodInvoker(delegate
            //{
            try
            {
                TreeViewItem nodeRoot = new TreeViewItem();
                treeReport.Items.Add(nodeRoot);


                TreeViewItem nodeLifeTimeLimitRoot = new TreeViewItem();
                nodeLifeTimeLimitRoot.Header = "Lifetime limits for this part";
                //nodeLifeTimeLimitRoot.Expand();
                TreeViewItem nodeLifeTimeLowerLimit = new TreeViewItem();
                nodeLifeTimeLowerLimit.Header = "Lower lifetime range limit [mV]: " + LifetimePartLimits.LowerRange.ToString();
                TreeViewItem nodeLifeTimeUpperLimit = new TreeViewItem();
                nodeLifeTimeUpperLimit.Header = "Upper lifetime range limit [mV]: " + LifetimePartLimits.UpperRange.ToString();
                nodeLifeTimeLimitRoot.Items.Add(nodeLifeTimeLowerLimit);
                nodeLifeTimeLimitRoot.Items.Add(nodeLifeTimeUpperLimit);

                nodeRoot.Items.Add(nodeLifeTimeLimitRoot);



                foreach (int Socket in LifetimeTest.TestResults.Keys)
                {
                    string CurrentSerialNumber = listLifetimeInfo1.GetSerialNumber(Socket);

                    TreeViewItem ItemserialNumber = new TreeViewItem();
                    ItemserialNumber.Header = listLifetimeInfo1.CurrentLifetimeInfo[Socket].SocketName + " -- " + CurrentSerialNumber;
                    //nodeRoot.Expand();
                    nodeRoot.Items.Add(ItemserialNumber);
                    foreach (int TestedDUTPin in LifetimeTest.TestResults[Socket].Keys)
                    {
                        double AverageVoltage = LifetimeTest.TestResults[Socket][TestedDUTPin].GetVoltageAverage();
                        double StdDev = LifetimeTest.TestResults[Socket][TestedDUTPin].GetStandardDeviation();


                        string strPin = string.Empty;
                        TreeViewItem nodePin;
                        nodePin = new TreeViewItem();
                        nodePin.Header = "DUT Pin " + TestedDUTPin.ToString();
                        TreeViewItem nodeMeasuredVoltage = new TreeViewItem();
                        nodeMeasuredVoltage.Header = "T" + numericTestHour.pinValue().ToString() + " Average Voltage = " + AverageVoltage.ToString();
                        TreeViewItem ItemstdDevVoltage = new TreeViewItem();
                        ItemstdDevVoltage.Header = "T" + numericTestHour.pinValue().ToString() + " Std Dev Voltage = " + StdDev.ToString();
                        TreeViewItem nodeIndividualMeasurements = new TreeViewItem();
                        nodeIndividualMeasurements.Header = "Individual Measurements (" + LifetimeTest.TestResults[Socket][TestedDUTPin].VoltageReadings.Count.ToString() + " values)";

                        //foreach (double Value in LifetimeTest.TestResults[Socket][TestedDUTPin].VoltageReadings)
                        //{
                        for (int i = 0; i < LifetimeTest.TestResults[Socket][TestedDUTPin].VoltageReadings.Count; i++)
                        {
                            nodeIndividualMeasurements.Items.Add("#" + (i + 1).ToString() + " = " + LifetimeTest.TestResults[Socket][TestedDUTPin].VoltageReadings[i].ToString());
                        }

                        ItemserialNumber.Items.Add(nodePin);
                     //   nodePin.Expand();

                        nodePin.Items.Add(nodeMeasuredVoltage);
                        nodePin.Items.Add(ItemstdDevVoltage);
                        nodePin.Items.Add(nodeIndividualMeasurements);

                        //nodePin.Expand();
                    }
                   // ItemserialNumber.Expand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DisplayTime0Data(): " + ex.ToString());
            }
            //}));
        }

        void CompareToLimits()
        {
            try
            {
                //if (this.InvokeRequired == true)
                //{
                //    this.Invoke(new MethodInvoker(delegate
                //    {
                TreeViewItem nodeRoot = new TreeViewItem();
                nodeRoot.Header = SelectedPartName + " - " + textBatchName.Text;
                treeReport.Items.Add(nodeRoot);


                TreeViewItem nodeLifeTimeLimitRoot = new TreeViewItem();
                nodeLifeTimeLimitRoot.Header = "Lifetime limits for this part";
                //nodeLifeTimeLimitRoot.Expand();
                TreeViewItem nodeLifeTimeLowerLimit = new TreeViewItem();
                nodeLifeTimeLowerLimit.Header = "Lower lifetime range limit [mV]: " + LifetimePartLimits.LowerRange.ToString();
                TreeViewItem nodeLifeTimeUpperLimit = new TreeViewItem();
                nodeLifeTimeUpperLimit.Header = "Upper lifetime range limit [mV]: " + LifetimePartLimits.UpperRange.ToString();
                nodeLifeTimeLimitRoot.Items.Add(nodeLifeTimeLowerLimit);
                nodeLifeTimeLimitRoot.Items.Add(nodeLifeTimeUpperLimit);

                nodeRoot.Items.Add(nodeLifeTimeLimitRoot);



                foreach (int Socket in LifetimeTest.TestResults.Keys)
                {
                    string CurrentSerialNumber = listLifetimeInfo1.GetSerialNumber(Socket);

                    TreeViewItem ItemserialNumber = new TreeViewItem();
                    ItemserialNumber.Header = listLifetimeInfo1.CurrentLifetimeInfo[Socket].SocketName + " -- " + CurrentSerialNumber;
                    //nodeRoot.Expand();
                    nodeRoot.Items.Add(ItemserialNumber);
                    int BaselineID = GCIDB.GetMostRecentLifetimeTestID_BaseLine(LoadedPartID, CurrentSerialNumber, textBatchName.Text.Trim());

                    if (BaselineID == 0)
                        return;

                    Dictionary<int, Double> BaselineData = GCIDB.GetLifetimeBaselineData(BaselineID);
                    foreach (int TestedDUTPin in LifetimeTest.TestResults[Socket].Keys)
                    {
                        double AverageVoltage = LifetimeTest.TestResults[Socket][TestedDUTPin].GetVoltageAverage();
                        double StdDev = LifetimeTest.TestResults[Socket][TestedDUTPin].GetStandardDeviation();
                        bool PinResult = false;
                        double UpperCompareValue = BaselineData[TestedDUTPin] + (LifetimePartLimits.UpperRange / 1000.0);
                        double LowerCompareValue = BaselineData[TestedDUTPin] + (LifetimePartLimits.LowerRange / 1000.0);

                        if (AverageVoltage < LowerCompareValue || AverageVoltage > UpperCompareValue)
                        {
                            PinResult = false;

                            listLifetimeInfo1.SetResult(Socket, false);
                          //  AddLog("Socket " + Socket + " DUT Pin [" + TestedDUTPin + "] Average Voltage: " + AverageVoltage + " FAILED");
                        }
                        else
                        {
                            PinResult = true;

                           // AddLog("Socket " + Socket + " DUT Pin [" + TestedDUTPin + "] Average Voltage: " + AverageVoltage + " PASSED");
                        }

                        string strPin = string.Empty;
                        TreeViewItem nodePin;
                        if (PinResult == true)
                        {
                            nodePin = new TreeViewItem();
                            nodePin.Header = "DUT Pin " + TestedDUTPin.ToString() + " (Passed)";
                            nodePin.Background = Brushes.Green;
                        }
                        else
                        {
                            nodePin = new TreeViewItem();
                            nodePin.Header = "DUT Pin " + TestedDUTPin.ToString() + " (Failed)";
                            nodePin.Background = Brushes.Red;
                        }


                        TreeViewItem nodeBaselineVoltage = new TreeViewItem();
                        nodeBaselineVoltage.Header = "Baseline (T0) Average Voltage = " + BaselineData[TestedDUTPin].ToString();
                        TreeViewItem nodeMeasuredVoltage = new TreeViewItem();
                        nodeMeasuredVoltage.Header = "T" + numericTestHour.pinValue().ToString() + " Average Voltage = " + AverageVoltage.ToString();
                        TreeViewItem ItemstdDevVoltage = new TreeViewItem();
                        ItemstdDevVoltage.Header = "T" + numericTestHour.pinValue().ToString() + " Std Dev Voltage = " + StdDev.ToString();
                        TreeViewItem nodeIndividualMeasurements = new TreeViewItem();
                        nodeIndividualMeasurements.Header = "Individual Measurements (" + LifetimeTest.TestResults[Socket][TestedDUTPin].VoltageReadings.Count.ToString() + " values)";

                        //foreach (double Value in LifetimeTest.TestResults[Socket][TestedDUTPin].VoltageReadings)
                        //{
                        for (int i = 0; i < LifetimeTest.TestResults[Socket][TestedDUTPin].VoltageReadings.Count; i++)
                        {
                            nodeIndividualMeasurements.Items.Add("#" + (i + 1).ToString() + " = " + LifetimeTest.TestResults[Socket][TestedDUTPin].VoltageReadings[i].ToString());
                        }

                        ItemserialNumber.Items.Add(nodePin);
                      //  nodePin.Expand();
                        nodePin.Items.Add(nodeBaselineVoltage);
                        nodePin.Items.Add(nodeMeasuredVoltage);
                        nodePin.Items.Add(ItemstdDevVoltage);
                        nodePin.Items.Add(nodeIndividualMeasurements);

                        //nodePin.Expand();
                    }
                   // ItemserialNumber.Expand();
                }

                //}));
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void Communication_OnResultComplete()
        {
            try
            {
                Double VoltageRef = Properties.Settings.Default.VoltageReference;
                int TestedPin = Communication.PinID1 + Communication.PinID2;
                Double Voltage = Math.Round((Communication.PinValue * VoltageRef) / 1023.0, 3);

                int BelongsToSocketId = GCItoDeviceIndex[TestedPin];
                int DUTPin = GCItoDUTMap[TestedPin];

                LifetimeTest.AddResult(BelongsToSocketId, DUTPin, Voltage);
                int NextPin = LifetimeTest.GetNextPin();
                if (NextPin != 0)
                {
                    Communication.TestPin(NextPin);
                }
                else
                {
                    if (numericTestHour.pinValue() > 0)
                    {
                       
                        CompareToLimits();   
                     
                        listLifetimeInfo1.DisplaAll(true);
                    }
                    else
                    {
                       
                            DisplayTime0Data();

                    }

                    //if (MessageBox.Show("Would you like to save this data to the database?", "Test Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    //{
                    //    SaveDataToDatabase();
                    //}
                    //SafeLockUI(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Communication_OnResultComplete(): " + ex.ToString());
            }
        }



        private void comboPartName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboPartName.Items.Count > 0)
                {
                    if (comboPartName.SelectedItem == null)
                        return;
                    SelectedPartName = comboPartName.SelectedItem.ToString();
                    PopulateBoardList();
                    listLifetimeInfo1.Clear();
                    LifetimePartLimits = GCIDB.GetLifetimeLimits(SelectedPartName);
                    LoadedLifetimeLimitID = LifetimePartLimits.LifetimeLimitID;
                    if (LifetimePartLimits.LifetimeLimitID > 0)
                    {
                        LoadedPartID = GCIDB.GetPartID(SelectedPartName);
                        ExistingSerialNumbers = GCIDB.GetSerialNumberList(SelectedPartName);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("comboPartName_SelectedIndexChanged(): " + ex.ToString());
            }
        }

        private void comboTestBoard_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboTestBoard.Items.Count > 0)
                {
                    if (comboTestBoard.SelectedItem == null)
                        return;
                    SelectedBoardName = comboTestBoard.SelectedItem.ToString();
                    LoadTestInformation();
                    //buttonStart.IsEnabled = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("comboTestBoard_SelectedIndexChanged(): " + ex.ToString());
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
            try
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
                listLifetimeInfo1.Clear();

                for (int i = 0; i < SlotNames.Count; i++)
                {
                    String Slot = SlotNames[i];
                    int Index = SocketIndex[i];
                    List<int> GCIPinsToTest = GetGCITestPinsFromIndex(Index);
                    listLifetimeInfo1.AddLifetimeTestSlot(Slot, Index, LoadedTestBoardID, GCIPinsToTest, ExistingSerialNumbers);
                }
                buttonStart.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadTestInformation(): " + ex.ToString());
            }
        }

        //private void SafeLockUI(bool State)
        //{
        //    if (this.InvokeRequired == true)
        //    {
        //        this.Invoke(new MethodInvoker(delegate
        //        {
        //            LockUI(State);
        //        }));
        //    }
        //    else
        //    {
        //        LockUI(State);
        //    }
        //}

        //private void LockUI(bool State)
        //{
        //    if (State == true)
        //    {
        //        comboPartName.Enabled = false;
        //        comboTestBoard.Enabled = false;
        //        numericIterations.Enabled = false;
        //        numericTemperature.Enabled = false;
        //        numericTestHour.Enabled = false;
        //        textBatchName.Enabled = false;
        //        buttonStart.Enabled = false;
        //    }
        //    else
        //    {
        //        comboPartName.Enabled = true;
        //        comboTestBoard.Enabled = true;
        //        numericIterations.Enabled = true;
        //        numericTemperature.Enabled = true;
        //        numericTestHour.Enabled = true;
        //        textBatchName.Enabled = true;
        //        buttonStart.Enabled = true;
        //    }
        //}

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //if (listLifetimeInfo1.CheckForDuplicateSerialNumber() == true)
            //{
            //    MessageBox.Show("Duplicate serial numbers detected, please fix.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //if (textBatchName.Text.Trim().Length == 0)
            //{
            //    MessageBox.Show("Please enter a batch name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            PinsToTest = listLifetimeInfo1.GetAllPinsToTest();
            int PinTotal = PinsToTest.Count;
            //ClearLog();
            treeReport.Items.Clear();
            LifetimeTest = new TestPart();
            LifetimeTest.TotalIterations = (int)numericIterations.pinValue();
            LifetimeTest.PinsToTest = PinsToTest;
           // SafeLockUI(true);
            if (PinTotal > 0)
            {
                Communication.TestPin(LifetimeTest.GetNextPin());
            }
        }





        //UPDATE THIS OSVALDO
        //private void frmLifetimeTest_FormClosing(object sender, EventArgs e)
        //{
        //    Communication.OnResultComplete -= new Communication.ResultComplete(Communication_OnResultComplete);
        //}
    }
}
