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
        public bool closed;
        public SelectDUTPins(List<int> InitialPins = null)
        {
            InitializeComponent();
            //PopulatePins(InitialPins);
        }

        //void PopulatePins(List<int> InitialPins)
        //{
        //    for (int i = 1; i <= 320; i++)
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

        List<int> GetPinsToTestList()
        {
            List<int> Results = new List<int>();
            for (int i = 0; i < Convert.ToInt32(numberofPins.Text); i++)
            {
                int PinID = i + 1;
                Results.Add(PinID);
            }
            return Results;
        }

        private void SelectDUTPins_Load(object sender, EventArgs e)
        {

        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            SelectedPins = GetPinsToTestList();
            //buttonAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            //this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void SelectDUTPins_Closed(object sender, EventArgs e)
        {
            closed = true;
        }
    }
}
