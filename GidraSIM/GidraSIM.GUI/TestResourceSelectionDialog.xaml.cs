using GidraSIM.GUI.Core.BlocksWPF;
using GidraSIM.GUI.Core.BlocksWPF.ViewModels.Resources;
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

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestResourceSelectionDialog.xaml
    /// </summary>
    public partial class TestResourceSelectionDialog : Window
    {
        public TestResourceSelectionDialog(Point position)
        {
            InitializeComponent();
            listBox1.Items.Add(new CadResourceViewModel(position, "CAD"));
            listBox1.Items.Add(new WorkerResourceViewModel(position, "Работник"));
            listBox1.Items.Add(new TechincalSupportResourceViewModel(position, "Рабочая станция"));
            listBox1.Items.Add(new MethodolgicalSupportResourceViewModel(position, "Методологическое обеспечение"));
            listBox1.SelectedIndex = 0;
            this.button.Focus();
        }

        public ResourceWPF SelectedResource { get; private set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SelectedResource = listBox1.SelectedItem as ResourceWPF;
            listBox1.Items.Remove(listBox1.SelectedItem);
            this.DialogResult = true;
        }
    }


}
