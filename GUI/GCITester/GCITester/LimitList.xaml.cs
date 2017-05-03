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
    /// Interaction logic for LimitList.xaml
    /// </summary>
    public partial class LimitList : UserControl
    {
        public List<Limits> CurrentList = new List<Limits>();
        public int PartID = 0;
        public int ProductionLimitID = 0;

        /*public delegate void LimitsEdited();
        public static event LimitsEdited OnLimitsEdited;*/

        public event EventHandler LimitsEdited;

        private void Limits_Edited(object sender, EventArgs e)
        {
            // invoke UserControl event here
            if (this.LimitsEdited != null) this.LimitsEdited(sender, e);
        }

        public void ChangeLimitsBySigmaAmount(int SigmaNumber)
        {
            foreach (Limits Limit in CurrentList)
            {
                Limit.ChangeLimitsBySigmaAmount(SigmaNumber);
            }
        }

        public bool HasEdits()
        {
            foreach (Limits Entry in CurrentList)
            {
                if (Entry.Edited == true)
                    return true;
            }
            return false;
        }

        public void ClearAllEdits()
        {
            foreach (Limits Entry in CurrentList)
            {
                Entry.Edited = false;
            }
        }

        public int TotalLimits
        {
            get
            {
                return CurrentList.Count;
            }
        }

        public List<int> GetCurrentDUTPins()
        {
            List<int> Result = new List<int>();
            foreach (Limits Item in CurrentList)
            {
                Result.Add(Item.DutPinID);
            }
            return Result;
        }

        public List<LimitEntity> BuildLimitEntityList()
        {
            List<LimitEntity> Result = new List<LimitEntity>();

            foreach (Limits Limit in CurrentList)
            {
                Result.Add(new LimitEntity(PartID, ProductionLimitID, Limit.UCL, Limit.LCL, Limit.DutPinID, Limit.AverageValue, Limit.StandardDeviation));
            }

            return Result;
        }

        public Dictionary<int, LimitEntity> BuildLimitEntityDictionary()
        {
            Dictionary<int, LimitEntity> Result = new Dictionary<int, LimitEntity>();

            foreach (Limits Limit in CurrentList)
            {
                Result.Add(Limit.DutPinID, new LimitEntity(PartID, ProductionLimitID, Limit.UCL, Limit.LCL, Limit.DutPinID, Limit.AverageValue, Limit.StandardDeviation));
            }

            return Result;
        }

        public void ClearLimits()
        {
           
                CurrentList = new List<Limits>();
                gridList.Children.Clear();

                Label NoPins = new Label();

                //NoPins.FontFamily = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((int)(0)));
                //NoPins.Location = new System.Drawing.Point(3, 0);
                //NoPins.Name = "labelNoPins";
                //NoPins.Size = new System.Drawing.Size(299, 120);
                NoPins.TabIndex = 0;
                NoPins.Content = "No pins are assigned to this device.  Click the \'Edit Test Pins\' button.";

                gridList.Children.Add(NoPins);

           
        }


        public void AddLimit(int DUTPin, Double LCL, Double UCL)
        {
            
                if (CurrentList.Count == 0)
                    gridList.Children.Clear();

                Limits LimitToAdd = new Limits();
                LimitToAdd.DutPinID = DUTPin;
                LimitToAdd.LCL = LCL;
                LimitToAdd.UCL = UCL;
               // LimitToAdd.DisplayStats = false;
           //     LimitToAdd.Top = (TotalLimits * 46) + 5;
               // this.AutoScrollMinSize = new Size(360, TotalLimits * 51);
                LimitToAdd.LimitsEdited += new EventHandler(Limits_Edited);
                CurrentList.Add(LimitToAdd);
                gridList.Children.Add(LimitToAdd);
           
        }

        public void AddLimit(int DUTPin, Double LCL, Double UCL, Double Average, Double StdDev)
        {
            
                if (CurrentList.Count == 0)
                    gridList.Children.Clear();
                Limits LimitToAdd = new Limits();
                LimitToAdd.DutPinID = DUTPin;
                LimitToAdd.LCL = LCL;
                LimitToAdd.UCL = UCL;
                LimitToAdd.AverageValue = Average;
                LimitToAdd.StandardDeviation = StdDev;
               // LimitToAdd.DisplayStats = true;
              //  LimitToAdd.Top = (TotalLimits * 46) + 5;
                //this.AutoScrollMinSize = new Size(360, TotalLimits * 51);
                LimitToAdd.LimitsEdited += new EventHandler(Limits_Edited);
                CurrentList.Add(LimitToAdd);
                gridList.Rows = TotalLimits;
               // listScroll.
                gridList.Children.Add(LimitToAdd);
         
        }

        public void AddLimit(LimitEntity Limit)
        {
            
                if (CurrentList.Count == 0)
                    gridList.Children.Clear();
                Limits LimitToAdd = new Limits();
                LimitToAdd.DutPinID = Limit.PinID;
                LimitToAdd.LCL = Limit.LCL;
                LimitToAdd.UCL = Limit.UCL;
                LimitToAdd.AverageValue = Limit.AverageVoltage;
                LimitToAdd.StandardDeviation = Limit.StdDevVoltage;
             //   if (Limit.AverageVoltage > 0)
                  //  LimitToAdd.DisplayStats = true;
              //  else
                   // LimitToAdd.DisplayStats = false;
               // LimitToAdd.Top = (TotalLimits * 46) + 5;
               // this.AutoScrollMinSize = new Size(360, TotalLimits * 51);
                LimitToAdd.LimitsEdited += new EventHandler(Limits_Edited);
                CurrentList.Add(LimitToAdd);
                gridList.Rows = TotalLimits;
                gridList.Children.Add(LimitToAdd);
           
        }

        public LimitList()
        {
            InitializeComponent();
        }

        private void LimitList_Load(object sender, EventArgs e)
        {

        }

       
    }
}
