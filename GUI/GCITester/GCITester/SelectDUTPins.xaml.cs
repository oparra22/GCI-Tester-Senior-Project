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
    /// Interaction logic for SelectDUTPins.xaml
    /// </summary>
    public partial class SelectDUTPins : Window
    {
        public List<int> SelectedPins = new List<int>();
        public SelectDUTPins(List<int> InitialPins = null)
        {
            InitializeComponent();
           // PopulatePins(InitialPins);
        }

        //void PopulatePins(List<int> InitialPins)
        //{
        //    for (int i = 1; i <= 105; i++)
        //    {
        //        if (InitialPins != null)
        //        {
        //            if (InitialPins.Contains(i))
        //                checkedListDUTPins.Items.Add(i.ToString(), true);
        //            else
        //                checkedListDUTPins.Items.Add(i.ToString(), false);
        //        }
        //        else
        //            checkedListDUTPins.Items.Add(i.ToString(), false);
        //    }


        //}

        //List<int> GetPinsToTestList()
        //{
        //    List<int> Results = new List<int>();
        //    for (int i = 0; i < checkedListDUTPins.CheckedItems.Count; i++)
        //    {
        //        int PinID = int.Parse(checkedListDUTPins.CheckedItems[i].ToString());
        //        Results.Add(PinID);
        //    }
        //    return Results;
        //}

        //private void frmSelectDUTPins_Load(object sender, EventArgs e)
        //{

        //}

        //private void buttonAccept_Click(object sender, EventArgs e)
        //{
        //    SelectedPins = GetPinsToTestList();
        //    buttonAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
        //    //this.DialogResult = System.Windows.Forms.DialogResult.OK;
        //    this.Close();
        //}

        //private void buttonCancel_Click(object sender, EventArgs e)
        //{
        //    buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        //    //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        //    this.Close();
        //}

        //private void buttonClearAll_Click(object sender, EventArgs e)
        //{
        //    for (int i = 0; i < checkedListDUTPins.Items.Count; i++)
        //    {
        //        checkedListDUTPins.SetItemChecked(i, false);
        //    }
        //}

        //private void buttonAll_Click(object sender, EventArgs e)
        //{
        //    for (int i = 0; i < checkedListDUTPins.Items.Count; i++)
        //    {
        //        checkedListDUTPins.SetItemChecked(i, true);
        //    }
        //}
    }
}
