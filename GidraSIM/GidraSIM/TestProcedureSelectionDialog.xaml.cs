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
using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Procedures;

namespace GidraSIM
{
    /// <summary>
    /// Логика взаимодействия для TestProcedureSelectionDialog.xaml
    /// </summary>
    public partial class TestProcedureSelectionDialog : Window
    {
        public TestProcedureSelectionDialog()
        {
            TokensCollector temp = new TokensCollector();
            InitializeComponent();
            listBox1.Items.Add(new FixedTimeBlock(temp,10));
            listBox1.SelectedIndex = 0;
            this.button.Focus();
        }

        public IBlock SelectedBlock { get => listBox1.SelectedItem as IBlock; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
