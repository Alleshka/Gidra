using System;
using System.Windows;

namespace GidraSIM.GUI.AdmSet
{
    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();

            Settings settings;
            //попытка чтения без рекурсии
            if(SettingsReader.TryRead(out settings))
            {
                _userPC.Text = settings.NamePC;
            }
            else
            {
                _userPC.Text = Environment.MachineName;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsReader.Save(new Settings()
            {
                NamePC = _userPC.Text
            });
            this.DialogResult = true;
            this.Close();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
