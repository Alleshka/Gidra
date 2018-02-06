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
using GidraSIM.Core.Model.Resources;

namespace GidraSim.BaseRedactor
{
    /// <summary>
    /// Логика взаимодействия для CPURedactor.xaml
    /// </summary>
    public partial class CPURedactor : Window
    {
        public CPURedactor()
        {
            InitializeComponent();
        }

        public CPU curCPU;

        private void SaveCPU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                curCPU = new CPU()
                {
                    Frequency = Convert.ToInt16(_frequency.Text),
                    QuantityCore = Convert.ToByte(_quantityCore.Text),
                    Price = Convert.ToDecimal(_price.Text)
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
