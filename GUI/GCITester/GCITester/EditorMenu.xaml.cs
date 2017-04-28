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
            PartEditor window = new PartEditor();
            window.Show();
            this.Close();
        }

        private void backToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.testsButton.IsEnabled = true;
            window.Show();
            this.Close();
        }
    }
}
