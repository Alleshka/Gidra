using System;
using System.Windows;

namespace GidraSIM.AdmSet
{
    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
            _userPC.Text = Environment.MachineName;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsReader.Save(new Settings()
            {
                NamePC = _userPC.Text
            });
            this.Close();
        }
    }
}
