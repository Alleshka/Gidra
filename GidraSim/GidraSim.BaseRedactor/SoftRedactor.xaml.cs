using GidraSIM.Core.Model.Resources;
using System;
using System.Windows;


namespace GidraSim.BaseRedactor
{
    /// <summary>
    /// Логика взаимодействия для SoftRedactor.xaml
    /// </summary>
    public partial class SoftRedactor : Window
    {
        public SoftRedactor()
        {
            InitializeComponent();
        }

        public Software curSoft;

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                curSoft = new Software()
                {
                    LicenseForm = (TypeLicenseForm)_LicenseForm.SelectedIndex,
                    LicenseStatus = _licenseStatus.Text,
                    Name = _name.Text,
                    Price = Convert.ToDecimal(_price.Text),
                    Type = (TypeSoftware)_Type.SelectedIndex
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
