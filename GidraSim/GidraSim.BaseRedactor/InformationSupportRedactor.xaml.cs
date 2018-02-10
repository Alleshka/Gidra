﻿using System;
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
using GidraSIM.Core.Model.Resources;


namespace GidraSim.BaseRedactor
{
    /// <summary>
    /// Логика взаимодействия для InformationSupportRedactor.xaml
    /// </summary>
    public partial class InformationSupportRedactor : Window
    {
        public InformationSupportRedactor()
        {
            InitializeComponent();
        }

        public InformationSupport curINF;

        private void InfSupSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                curINF = new InformationSupport()
                {
                    Price = Convert.ToDecimal(_price.Text),
                    Type = (TypeIS)type.SelectedIndex,
                    MultiClientUse = (bool)_MultiClientUse.IsChecked       
                };

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}