using GidraSIM.Core.Model.Resources;
using System;
using System.Windows;


namespace GidraSim.BaseRedactor
{
    /// <summary>
    /// Логика взаимодействия для MonitorRedactor.xaml
    /// </summary>
    public partial class MonitorRedactor : Window
    {
        public MonitorRedactor()
        {
            InitializeComponent();
        }

        public Monitor curMonitor;

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                curMonitor = new Monitor()
                {
                    Diagonal = Convert.ToByte(_diagonal.Text),
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
