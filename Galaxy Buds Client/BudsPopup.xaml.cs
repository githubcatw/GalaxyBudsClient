﻿using Galaxy_Buds_Client.model.Constants;
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

namespace Galaxy_Buds_Client {
    /// <summary>
    /// Interaction logic for BudsPopup.xaml
    /// </summary>
    public partial class BudsPopup : Window {
        public BudsPopup(Model model) {
            InitializeComponent();
            string mod = "";
            if (model == Model.BudsPlus) mod = "+";
            string name = Environment.UserName.Split(' ')[0];
            Greeting.Content = $"{name}'s Galaxy Buds{mod}";
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            Close();
        }
    }
}