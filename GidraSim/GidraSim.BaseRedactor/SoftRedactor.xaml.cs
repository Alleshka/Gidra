using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;
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

            _Type.Items.Add(EnumExtension.Description<TypeSoftware>(TypeSoftware.OS));
            _Type.Items.Add(EnumExtension.Description<TypeSoftware>(TypeSoftware.Redactor));
            _Type.Items.Add(EnumExtension.Description<TypeSoftware>(TypeSoftware.CAD));

            _LicenseForm.Items.Add(EnumExtension.Description<TypeLicenseForm>(TypeLicenseForm.Commerc));
            _LicenseForm.Items.Add(EnumExtension.Description<TypeLicenseForm>(TypeLicenseForm.Free));
            _LicenseForm.Items.Add(EnumExtension.Description<TypeLicenseForm>(TypeLicenseForm.Open));
            _LicenseForm.Items.Add(EnumExtension.Description<TypeLicenseForm>(TypeLicenseForm.ShareWare));
        }

        public Software curSoft;

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                curSoft = new Software()
                {
                    LicenseForm = EnumExtension.GetEnum<TypeLicenseForm>("ОС"),
                    LicenseStatus = _licenseStatus.Text,
                    Name = _name.Text,
                    Price = Convert.ToDecimal(_price.Text),
                    Type = EnumExtension.GetEnum<TypeSoftware>(_Type.SelectedIndex.ToString())
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
