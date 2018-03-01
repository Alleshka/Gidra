using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;
using System;
using System.Windows;

namespace GidraSim.BaseRedactor
{
    /// <summary>
    /// Логика взаимодействия для StorageRedactor.xaml
    /// </summary>
    public partial class StorageRedactor : Window
    {
        public StorageRedactor()
        {
            InitializeComponent();
        }

        public StorageDevice _curStorage;

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _curStorage = new StorageDevice()
                {
                    SpeedRead = Convert.ToInt16(_speedRead.Text),
                    SpeedWrite = Convert.ToInt16(_speedWrite.Text),
                    Size = Convert.ToInt16(_size.Text),
                    Price = Convert.ToDecimal(_price.Text)
                };
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
