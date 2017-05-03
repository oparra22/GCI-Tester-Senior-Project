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
using System.ComponentModel;
using System.Drawing;
using System.Data;

//using System.Windows.Forms;

namespace GCITester
{
    public partial class TestBoardList : UserControl
    {
        public List<TestPinMap> CurrentPinMap = new List<TestPinMap>();
        public int PartID = 0;
        public int TestBoardID = 0;
        public int ExtraSpace = 0;

        public int TotalPins
        {
            get
            {
                return CurrentPinMap.Count;
            }
        }

        public bool HasEdits()
        {
            foreach (TestPinMap Entry in CurrentPinMap)
            {
                if (Entry.Edited == true)
                    return true;
            }
            return false;
        }

        public void ClearAllEdits()
        {
            foreach (TestPinMap Entry in CurrentPinMap)
            {
                Entry.Edited = false;
            }
        }


        /*public List<Byte> GetCurrentDUTPins()
        {
            List<Byte> Result = new List<byte>();
            foreach (Limits Item in CurrentList)
            {
                Result.Add(Item.DutPinID);
            }
            return Result;
        }*/

        /*public List<LimitEntity> BuildLimitEntityList()
        {
            List<LimitEntity> Result = new List<LimitEntity>();

            foreach (Limits Limit in CurrentList)
            {
                Result.Add(new LimitEntity(PartID, ProductionLimitID, Limit.UCL, Limit.LCL, Limit.DutPinID, Limit.AverageValue, Limit.StandardDeviation));
            }

            return Result;
        }*/

        /*public Dictionary<byte, LimitEntity> BuildLimitEntityDictionary()
        {
            Dictionary<byte, LimitEntity> Result = new Dictionary<byte, LimitEntity>();

            foreach (Limits Limit in CurrentList)
            {
                Result.Add(Limit.DutPinID, new LimitEntity(PartID, ProductionLimitID, Limit.UCL, Limit.LCL, Limit.DutPinID, Limit.AverageValue, Limit.StandardDeviation));
            }

            return Result;
        }*/

        public void ClearPins()
        {
            
            
                CurrentPinMap = new List<TestPinMap>();
                ExtraSpace = 0;
            // this.Controls.Clear();
            TestBoardListGrid.Children.Clear();
        }

        public void AddPinMap(int PartID, int TestBoardID, String DeviceName, int DUTPin, int GCIPin)
        {
            
                TestPinMap PinToAdd = new TestPinMap(PartID, TestBoardID, DeviceName, DUTPin, GCIPin);
                PinToAdd.Margin = new Thickness(0,5,0,0);
               
                //this.AutoScrollMinSize = new Size(513, TotalPins * 30 + ExtraSpace);
                CurrentPinMap.Add(PinToAdd);
            // this.Controls.Add(PinToAdd);
                TestBoardListGrid.Children.Add(PinToAdd);
            
            //TestBoardListGrid.UpdateLayout();
        }

        public void AddPinMap(TestPinEntity PinMap)
        {
            int colCount;
                TestPinMap PinToAdd = new TestPinMap(PinMap.PartID, PinMap.TestBoardID, PinMap.SocketName, PinMap.DUTPin, PinMap.GCIPin);
            PinToAdd.Margin = new Thickness(0, 5, 0,0);
           // PinToAdd = new Size(513, TotalPins * 30 + ExtraSpace);
            // PinToAdd.Visibility = (TotalPins * 30) + 5 + ExtraSpace;
            // PinToAdd.Content = (TotalPins * 30) + 5 + ExtraSpace;
            //PinToAdd.AutoScrollMinSize = new Size(513, TotalPins * 30 + ExtraSpace);
            CurrentPinMap.Add(PinToAdd);
               // this.Controls.Add(PinToAdd);
            TestBoardListGrid.Children.Add(PinToAdd);
            // colCount += colCount;
            // TestBoardListGrid.Columns.Equals(colCount);
           // TestBoardListGrid.Rows.Equals(4);
           
        }

        public TestBoardList()
        {
            InitializeComponent();
        }

        private void TestBoardList_Load(object sender, EventArgs e)
        {

        }
    }
}
