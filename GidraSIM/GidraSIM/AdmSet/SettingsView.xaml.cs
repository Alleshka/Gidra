using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
