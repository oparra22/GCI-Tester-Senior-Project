﻿using System;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GCITester
{

    public partial class PartEditor : Window
    {

        public PartEditor()
        {
            InitializeComponent();
        }

        private void addButtonPartButton_Click(object sender, RoutedEventArgs e)
        {
            AddPartName window = new AddPartName();
            window.Show();
        }

        public void PartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PartsList_Loaded(object sender, RoutedEventArgs e)
        {
            PartsList.Items.Add("Seth eats Ass");
        }

        private void addButtonPartButton_Copy_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }

}