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
    /// Interaction logic for listProductionInfo.xaml
    /// </summary>
    public partial class listProductionInfo : UserControl
    {
        public List<itemProductionInfo> CurrentProductionInfo = new List<itemProductionInfo>();
        public int PartID = 0;
        public int TestBoardID = 0;

        public int Total
        {
            get
            {
                return CurrentProductionInfo.Count;
            }
        }

        public void Clear()
        {
         
                CurrentProductionInfo = new List<itemProductionInfo>();
                listGrid.Children.Clear();
         
        }

        public void ResetAll()
        {
            foreach (itemProductionInfo Item in CurrentProductionInfo)
            {
                Item.DisplayResult(false);
                Item.ResetTest();
            }
        }

        public void DisplaAll(bool Display)
        {
            foreach (itemProductionInfo Item in CurrentProductionInfo)
            {
                Item.DisplayResult(Display);
            }
        }

        public void SetResult(int SocketIndexID, bool Result)
        {
            foreach (itemProductionInfo Item in CurrentProductionInfo)
            {
                if (Item.SocketIndex == SocketIndexID)
                {
                    Item.Result = Result;
                    return;
                }
            }
        }
        public List<int> GetAllPinsToTest()
        {
            List<int> Result = new List<int>();
            foreach (itemProductionInfo Item in CurrentProductionInfo)
            {
                if (Item.SkipTest == false)
                {
                    foreach (int Pin in Item.PinsToTest)
                    {
                        Result.Add(Pin);
                    }
                }
            }
            return Result;
        }

        public void AddProductionTestSlot(String SocketName, int SocketIndex, int TestBoardID, List<int> PinsToTest)
        {
            
            
                itemProductionInfo SlotToAdd = new itemProductionInfo(SocketName, SocketIndex, TestBoardID, PinsToTest);
               // SlotToAdd.Top = (Total * 50) + 5;
               // this.AutoScrollMinSize = new Size(339, Total * 50);
                CurrentProductionInfo.Add(SlotToAdd);
                listGrid.Children.Add(SlotToAdd);
           
        }

        public listProductionInfo()
        {
            InitializeComponent();
           
        }

        private void ListProductionInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
