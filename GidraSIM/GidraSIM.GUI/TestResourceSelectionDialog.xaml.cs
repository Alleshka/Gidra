using GidraSIM.Core.Model.Resources;
using GidraSIM.GUI.Core.BlocksWPF;
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
using System.ComponentModel;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestResourceSelectionDialog.xaml
    /// </summary>
    public partial class TestResourceSelectionDialog : Window
    {
        private TestResourceViewModel model;
        public TestResourceSelectionDialog()
        {
            InitializeComponent();
            model = new TestResourceViewModel();
            this.DataContext = model;
            listBox1.SelectedIndex = 0;
            this.button.Focus();
        }

        public AbstractResource SelectedResource { get; private set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SelectedResource = listBox1.SelectedItem as AbstractResource;
            //listBox1.Items.Remove(listBox1.SelectedItem);
            this.DialogResult = true;
        }
    }

    public class TestResourceViewModel : INotifyPropertyChanged
    {
        public TestResourceViewModel()
        {
            Blocks = new List<AbstractResource>()
            {
             new CadResource(),
             new WorkerResource(),
             new TechincalSupportResource(),
             new MethodolgicalSupportResource(),
            };
        }

        private List<AbstractResource> blocks;

        public List<AbstractResource> Blocks
        {
            get => blocks;
            set
            {
                blocks = value;
                NotifyPropertyChanged("Blocks");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}