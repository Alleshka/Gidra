using System.Collections.Generic;
using System.Windows;
using GidraSIM.Core.Model;
using GidraSIM.GUI.Core.BlocksWPF;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestSubProcessDialog.xaml
    /// </summary>
    public partial class SubProcessSelectionDialog : Window
    {
        private Point point;
        public SubProcessSelectionDialog(Point position, List<Process> allProcesses)
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
            SelectedProcess = new SubProcessWPF(point, process);
            listBox1.Items.Remove(listBox1.SelectedItem);
            this.DialogResult = true;
        }
    }
}
