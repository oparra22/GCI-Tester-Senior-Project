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
        //Dictionary<Byte, LearnResult> LearnResults;
        //List<LearnControl> LearnControl;
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
            //LearnResults = new Dictionary<byte, LearnResult>();
            //LearnControl = new List<LearnControl>();
            //List<Byte> PinsToTest = GCIToDUTMap.Keys.ToList<Byte>();
            //CurrentDeviceNumber = 0;
            //TotalDevices = (int)numericNumberOfParts.Value;
            //int TotalIterations = (int)numericIterations.Value;

            //LearnControl ControlSetup = new LearnControl();
            //ControlSetup.PinsToTest = PinsToTest;
            //ControlSetup.TotalIterations = TotalIterations;
            //for (int i = 0; i < TotalDevices; i++)
            //{
            //    LearnControl.Add(ControlSetup);
            //}

            //Byte NextPin = LearnControl[CurrentDeviceNumber].GetNextPin();

            //SetButtonState(false);

            //if (NextPin != 0)
            //    Communication.TestPin(NextPin);

        }

        private void cancel_click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = Window.DialogResult.Cancel;
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
        //void Communication_OnResultComplete()
        //{
        //    Double VoltageRef = Properties.Settings.Default.VoltageReference;
        //    Byte TestedPin = Communication.PinID;
        //    Double Voltage = (Communication.PinValue * VoltageRef) / 1023.0;

        //    if (LearnResults.ContainsKey(TestedPin))
        //    {
        //        LearnResults[TestedPin].VoltageReadings.Add(Voltage);
        //    }
        //    else
        //    {
        //        LearnResults.Add(TestedPin, new LearnResult(TestedPin, Voltage));
        //    }


        //    if (CurrentDeviceNumber < LearnControl.Count)
        //    {
        //        Byte NextPin = LearnControl[CurrentDeviceNumber].GetNextPin();
        //        UpdateCurrentIteration(true);
        //        if (NextPin != 0)
        //        {
        //            //UpdateCurrentIteration(true);
        //            Communication.TestPin(NextPin);
        //        }
        //        else
        //        {
        //            CurrentDeviceNumber++;
        //            if (CurrentDeviceNumber != LearnControl.Count)
        //            {
        //                if (MessageBox.Show("Please place part #" + (CurrentDeviceNumber + 1).ToString() + " in the socket", "Next Part", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
        //                {
        //                    NextPin = LearnControl[CurrentDeviceNumber].GetNextPin();
        //                    Communication.TestPin(NextPin);
        //                }
        //            }
        //            else
        //            {
        //                UpdateCurrentIteration(false);
        //                SetButtonState(true);
        //                BuildReport();
        //                MessageBox.Show("Testing complete");
        //            }

        //        }

        //    }

        //}

        //private void SetButtonState(bool Enabled)
        //{
        //    this.Invoke(new MethodInvoker(delegate
        //    {
        //        buttonSave.Enabled = Enabled;
        //        buttonStart.Enabled = Enabled;
        //    }));
        //}

        //private void BuildReport()
        //{
        //    this.Invoke(new MethodInvoker(delegate
        //    {
        //        treeResults.Nodes.Clear();
        //        TreeNode Root = new TreeNode(SelectedPartName);
        //        treeResults.Nodes.Add(Root);

        //        foreach (Byte PinID in LearnResults.Keys)
        //        {
        //            TreeNode Name = new TreeNode("Pin " + GCIToDUTMap[PinID].ToString());
        //            TreeNode Average = new TreeNode("Average: " + LearnResults[PinID].GetVoltageAverage() + " V");
        //            TreeNode StdDev = new TreeNode("StdDev: " + LearnResults[PinID].GetStandardDeviation() + " V");
        //            TreeNode MeasuredValues = new TreeNode(LearnResults[PinID].VoltageReadings.Count.ToString() + " Measured Voltages");
        //            for (int i = 0; i < LearnResults[PinID].VoltageReadings.Count; i++)
        //            {
        //                TreeNode Value = new TreeNode("Iteration " + (i + 1).ToString() + ") " + LearnResults[PinID].VoltageReadings[i].ToString());
        //                MeasuredValues.Nodes.Add(Value);
        //            }
        //            Name.Nodes.Add(Average);
        //            Name.Nodes.Add(StdDev);
        //            Name.Nodes.Add(MeasuredValues);
        //            Root.Nodes.Add(Name);
        //        }
        //    }));
        //}

        //private void UpdateCurrentIteration(bool Visible)
        //{
        //    this.Invoke(new MethodInvoker(delegate
        //    {
        //        if (Visible == true)
        //            labelProgress.Visible = true;
        //        else
        //        {
        //            labelProgress.Visible = false;
        //            return;
        //        }
        //        labelProgress.Text = "Progress (" + (CurrentDeviceNumber + 1).ToString() + " / " + TotalDevices.ToString() + ")  Pin (" + (LearnControl[CurrentDeviceNumber].CurrentPinNumber + 1).ToString() + "/" + LearnControl[CurrentDeviceNumber].PinsToTest.Count.ToString() + ")";
        //    }));
        //}

        private void productionLimits_OnClosing(object sender, CancelEventArgs e)
        {
            //Communication.OnResultComplete -= new Communication.ResultComplete(Communication_OnResultComplete);
        }


    }
}
