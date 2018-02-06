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
    /// Логика взаимодействия для GPURedactor.xaml
    /// </summary>
    public partial class GPURedactor : Window
    {
        public GPURedactor()
        {
            InitializeComponent();
        }

        public GPU curGPU;

        private void SaveGPU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                curGPU = new GPU()
                {
                    Frequency = Convert.ToInt16(_frequency.Text),
                    Memory = Convert.ToInt16(_memory.Text),
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
