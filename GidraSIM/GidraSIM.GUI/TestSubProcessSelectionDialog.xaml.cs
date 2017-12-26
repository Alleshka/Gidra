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
using GidraSIM.GUI.Core.BlocksWPF;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestSubProcessDialog.xaml
    /// </summary>
    public partial class TestSubProcessSelectionDialog : Window
    {
        private Point point;
        public TestSubProcessSelectionDialog(Point position, List<Process> allProcesses)
        {
            InitializeComponent();
            foreach(var process in allProcesses)
            {
                listBox1.Items.Add(process);
            }

            listBox1.SelectedIndex = 0;
            this.button.Focus();
            point = position;
        }

        public SubProcessWPF SelectedProcess { get; set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var process = (listBox1.SelectedItem as Process);
            SelectedProcess = new SubProcessWPF(point, process.Description);
            SelectedProcess.ProcedurePrototype = process;
            listBox1.Items.Remove(listBox1.SelectedItem);
            this.DialogResult = true;
        }
    }
}
