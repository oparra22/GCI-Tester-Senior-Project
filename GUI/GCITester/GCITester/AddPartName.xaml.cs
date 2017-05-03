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
    /// Interaction logic for AddPartName.xaml
    /// </summary>
    public partial class AddPartName : Window
    {
        public String PartName = string.Empty;

        public AddPartName()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            PartName = textPartName.Text;
          //  buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
           // this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
          //  buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          //  this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

       
    }
}
