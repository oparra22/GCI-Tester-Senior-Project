using System;
using System.Collections.ObjectModel;
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
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data;
namespace GCITester
{

    public partial class PartEditor : Window
    {
        string SelectedPartName = string.Empty;
        List<LimitEntity> LoadedLimitData = new List<LimitEntity>();
        LifetimeLimitEntity LoadedLifetimeLimitData = new LifetimeLimitEntity();
        int SelectedPartID = 0;
        int SelectedProductionLimitID = 0;
        int SelectedLifetimeLimitID = 0;
        bool PinsEdited = false;


        public PartEditor()
        {
            InitializeComponent();
        }

        private void PopulatePartList()
        {
            listParts.Items.Clear();
            List<string> PartNames = GCIDB.GetPartList();
            foreach (string part in PartNames)
            {
                listParts.Items.Add(part);
            }
        }

        private void PartEditor_Load(object sender, EventArgs e)
        {
            GCIDB.Initialize();
            PopulatePartList();
            lifetimeLimits1.SetEnabled(false);
        }

        private void listParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PinsEdited == true || limitList1.HasEdits() == true || lifetimeLimits1.Edited == true)
            {
               // if (MessageBox.Show("There were changes made to the part.\nWould you like to save these changes to the database?", "Edits detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
               // {
                    SaveChanges();
                //}
            }
            PinsEdited = false;
            limitList1.ClearAllEdits();
            lifetimeLimits1.Edited = false;
           // buttonSaveChanges.Enabled = false;

            if (listParts.Items.Count > 0)
            {
                if (listParts.SelectedItem == null)
                    return;
                SelectedPartName = listParts.SelectedItem.ToString();
                SelectedLifetimeLimitID = GCIDB.GetLifetimeLimitID(SelectedPartName);
                LoadedLimitData = GCIDB.GetProductionLimits(SelectedPartName);
                lifetimeLimits1.SetEnabled(true);

                if (SelectedLifetimeLimitID > 0)
                {
                    LoadedLifetimeLimitData = GCIDB.GetLifetimeLimits(SelectedPartName);
                    lifetimeLimits1.LCL = LoadedLifetimeLimitData.LowerRange;
                    lifetimeLimits1.UCL = LoadedLifetimeLimitData.UpperRange;
                }
                else
                {
                    lifetimeLimits1.LCL = Properties.Settings.Default.LifetimeLimit_DefaultLowerRange;
                    lifetimeLimits1.UCL = Properties.Settings.Default.LifetimeLimit_DefaultUpperRange;
                }

                if (LoadedLimitData.Count > 0)
                {
                    SelectedPartID = LoadedLimitData[0].PartID;
                    SelectedProductionLimitID = LoadedLimitData[0].ProductionLimitID;
                }
                else
                {
                    SelectedPartID = GCIDB.GetPartID(SelectedPartName);
                    SelectedProductionLimitID = 0;
                }
                limitList1.ClearLimits();
                foreach (LimitEntity Limit in LoadedLimitData)
                {
                    limitList1.AddLimit(Limit);
                }
               // buttonEditTestPins.Enabled = true;
                //buttonSaveChanges.Enabled = true;
            }

        }

        //private void buttonEditTestPins_Click(object sender, EventArgs e)
        //{
        //    List<Byte> CurrentPins = limitList1.GetCurrentDUTPins();

        //    frmSelectDUTPins DutPinSelect = new frmSelectDUTPins(CurrentPins);
        //    if (DutPinSelect.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
        //    {
        //        buttonSaveChanges.Enabled = true;
        //        PinsEdited = true;
        //        List<byte> DUTPinResults = DutPinSelect.SelectedPins;
        //        List<byte> ExistingPins = limitList1.GetCurrentDUTPins();

        //        List<byte> MergedPins = new List<byte>();

        //        foreach (Byte Pin in DUTPinResults)
        //        {
        //            if (MergedPins.Contains(Pin) == false)
        //                MergedPins.Add(Pin);
        //        }

        //        foreach (Byte Pin in ExistingPins)
        //        {
        //            if (MergedPins.Contains(Pin) == false)
        //                MergedPins.Add(Pin);
        //        }


        //        Dictionary<byte, LimitEntity> CurrentLimitDict = limitList1.BuildLimitEntityDictionary();

        //        foreach (Byte Pin in MergedPins)
        //        {
        //            if (CurrentLimitDict.ContainsKey(Pin) == false)
        //            {
        //                CurrentLimitDict.Add(Pin, new LimitEntity(SelectedPartID, SelectedProductionLimitID, 0, 0, Pin, 0, 0));
        //            }
        //        }

        //        limitList1.ClearLimits();


        //        foreach (byte Pin in CurrentLimitDict.Keys)
        //        {
        //            if (DUTPinResults.Contains(Pin))
        //                limitList1.AddLimit(CurrentLimitDict[Pin]);
        //        }
        //    }
        //}

        private void buttonAddNewPart_Click(object sender, EventArgs e)
        {
            AddPartName AddPart = new AddPartName();
           // if (AddPart.ShowDialog() == System.Windows.Forms.DialogResult.OK)
           // {
                int PartId = GCIDB.GetPartID(AddPart.PartName);
                if (PartId == 0)
                {
                    GCIDB.AddPartID(AddPart.PartName);
                    int LifetimeLimitID = GCIDB.AddNewLifetimeLimit(AddPart.PartName, Properties.Settings.Default.LifetimeLimit_DefaultLowerRange, Properties.Settings.Default.LifetimeLimit_DefaultUpperRange);
                    GCIDB.SetLifetimeLimit(AddPart.PartName, LifetimeLimitID);
                    PopulatePartList();
                }
               // else
                    //MessageBox.Show("Error partname already exists in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
       }

        private void SaveChanges()
        {
            if (PinsEdited == true || limitList1.HasEdits() == true)
            {
                SelectedProductionLimitID = GCIDB.AssociatePartToNewProductionLimit(SelectedPartName);
                List<LimitEntity> UserLimits = limitList1.BuildLimitEntityList();
                foreach (LimitEntity entry in UserLimits)
                {
                    GCIDB.AddProductionLimit(SelectedProductionLimitID, entry.PinID, entry.UCL, entry.LCL, entry.AverageVoltage, entry.StdDevVoltage);
                }
                PinsEdited = false;
                limitList1.ClearAllEdits();
            //    buttonSaveChanges.Enabled = false;
            }

            if (lifetimeLimits1.Edited == true)
            {
                int LifetimeLimitID = GCIDB.AddNewLifetimeLimit(SelectedPartName, lifetimeLimits1.LCL, lifetimeLimits1.UCL);
                SelectedLifetimeLimitID = LifetimeLimitID;
                GCIDB.SetLifetimeLimit(SelectedPartName, SelectedLifetimeLimitID);
                lifetimeLimits1.Edited = false;
               // buttonSaveChanges.Enabled = false;
            }
            MessageBox.Show("All changes saved!");
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void buttonSigmaApply_Click(object sender, EventArgs e)
        {
            limitList1.ChangeLimitsBySigmaAmount((int)numericSigma.pinValue());
        }

        private void buttonLearn_Click(object sender, EventArgs e)
        {
        }

        private void buttonDeletePart_Click(object sender, EventArgs e)
        {
          //  if (MessageBox.Show("Are you sure you want to delete the part " + SelectedPartName + "?", "Are you really sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
          //  {
                GCIDB.DeletePart(SelectedPartName);
                limitList1.ClearLimits();
                lifetimeLimits1.UCL = 0;
                lifetimeLimits1.LCL = 0;
                PopulatePartList();
          //  }
        }

        //private void limitList1_LimitsEdited(object sender, EventArgs e)
        //{
        //    buttonSaveChanges.Enabled = true;
        //}

        //private void lifetimeLimits1_LimitsEdited(object sender, EventArgs e)
        //{
        //    buttonSaveChanges.Enabled = true;
        //}

        private void lifetimeLimits1_Load(object sender, RoutedEventArgs e)
        {

        }

      










        //private void frmPartEditor_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (PinsEdited == true || limitList1.HasEdits() == true || lifetimeLimits1.Edited == true)
        //    {
        //        if (MessageBox.Show("There were changes made to the part.\nWould you like to save these changes to the database?", "Edits detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
        //        {
        //            SaveChanges();
        //        }
        //    }
        //}

    }

}