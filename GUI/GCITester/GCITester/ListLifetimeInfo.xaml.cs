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
    /// Interaction logic for ListLifetimeInfo.xaml
    /// </summary>
    public partial class ListLifetimeInfo : UserControl
    {
        public List<ItemLifetimeInfo> CurrentLifetimeInfo = new List<ItemLifetimeInfo>();
        public int PartID = 0;
        public int TestBoardID = 0;

        public int Total
        {
            get
            {
                return CurrentLifetimeInfo.Count;
            }
        }

        public void Clear()
        {
             CurrentLifetimeInfo = new List<ItemLifetimeInfo>();
             listGrid.Children.Clear();
           
        }

        public void DisplaAll(bool Display)
        {
            foreach (ItemLifetimeInfo Item in CurrentLifetimeInfo)
            {
                Item.DisplayResult(Display);
            }
        }


        public void ResetAll()
        {
            foreach (ItemLifetimeInfo Item in CurrentLifetimeInfo)
            {
                Item.DisplayResult(false);
                Item.ResetTest();
            }
        }

        public void SetResult(int SocketIndexID, bool Result)
        {
            foreach (ItemLifetimeInfo Item in CurrentLifetimeInfo)
            {
                if (Item.SocketIndex == SocketIndexID)
                {
                    Item.Result = Result;
                    return;
                }
            }
        }

        public bool CheckForDuplicateSerialNumber()
        {
            for (int i = 0; i < CurrentLifetimeInfo.Count; i++)
            {
                String SerialNumber1 = CurrentLifetimeInfo[i].SerialNumber;
                for (int j = 0; j < CurrentLifetimeInfo.Count; j++)
                {
                    if (i != j)
                    {
                        String SerialNumber2 = CurrentLifetimeInfo[j].SerialNumber;
                        if (SerialNumber1 == SerialNumber2)
                            return true;
                    }
                }

            }
            return false;
        }

        public List<int> GetAllPinsToTest()
        {
            List<int> Result = new List<int>();
            foreach (ItemLifetimeInfo Item in CurrentLifetimeInfo)
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

        public String GetSerialNumber(int SocketIndex)
        {
            foreach (ItemLifetimeInfo Item in CurrentLifetimeInfo)
            {
                if (Item.SocketIndex == SocketIndex)
                {
                    return Item.SerialNumber;
                }
            }
            return string.Empty;
        }

        public void AddLifetimeTestSlot(String SocketName, int SocketIndex, int TestBoardID, List<int> PinsToTest, List<String> SerialNumbers)
        {
             ItemLifetimeInfo SlotToAdd = new ItemLifetimeInfo(SocketName, SocketIndex, TestBoardID, PinsToTest, SerialNumbers);
            // SlotToAdd.Top = (Total * 50) + 5;
             //Grid.AutoScrollMinSize = new Size(529, Total * 50);
            CurrentLifetimeInfo.Add(SlotToAdd);
            //this.Controls.Add(SlotToAdd);
            //Canvas.SetTop(SlotToAdd, 200);
           // Canvas.SetLeft(SlotToAdd, 0);
           // listCanvas.Children.Add(SlotToAdd);
            listGrid.Children.Add(SlotToAdd);
           
            
            
        }

        public ListLifetimeInfo()
        {
            InitializeComponent();
        }
    }
}
