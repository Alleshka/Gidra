using GidraSIM.Core.Model.Resources;
using System.Collections.Generic;
using System.Windows;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestResourceSelectionDialog.xaml
    /// </summary>
    public partial class TestResourceSelectionDialog : Window
    {
        public TestResourceSelectionDialog()
        {
            InitializeComponent();
            listBox1.Items.Add(new CadResource());
            listBox1.Items.Add(new WorkerResource());
            listBox1.Items.Add(new TechincalSupportResource());
            listBox1.Items.Add(new MethodolgicalSupportResource());
            listBox1.SelectedIndex = 0;
            SelectedResource = new List<AbstractResource>();
            this.button.Focus();
        }

        public List<AbstractResource> SelectedResource { get; private set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            foreach (AbstractResource item in listBox1.SelectedItems) SelectedResource.Add(item);
            //listBox1.Items.Remove(listBox1.SelectedItem);
            this.DialogResult = true;
        }
    }
}