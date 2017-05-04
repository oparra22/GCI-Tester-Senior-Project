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
    /// Interaction logic for EditorMenu.xaml
    /// </summary>
    public partial class EditorMenu : Window
    {
        public EditorMenu()
        {
            InitializeComponent();
        }

        private void partEditor_Click(object sender, RoutedEventArgs e)
        {
            PartEditor PartEdit = new PartEditor();
            PartEdit.ShowDialog();

        }

        private void backToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Main = new MainWindow();
            Main.testsButton.IsEnabled = true;
            Main.ShowDialog();
           
        }

        private void boarEditor_Click(object sender, RoutedEventArgs e)
        {
            TestEditor TestEdit = new TestEditor();
            TestEdit.ShowDialog();
        }
    }
}
