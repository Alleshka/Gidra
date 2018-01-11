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
using GidraSIM.GUI.Core.BlocksWPF;
using System.ComponentModel;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestProcedureSelectionDialog.xaml
    /// </summary>
    public partial class TestProcedureSelectionDialog : Window
    {
        TestProcedureViewModel model;
        public TestProcedureSelectionDialog()
        {
            InitializeComponent();

            model = new TestProcedureViewModel();
            this.DataContext = model;
            this.button.Focus();
            listBox1.SelectedIndex = 0;
        }

        public IBlock SelectedBlock { get; private set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SelectedBlock = listBox1.SelectedItem as IBlock;
            this.DialogResult = true;
        }
    }


    public class TestProcedureViewModel : INotifyPropertyChanged
    {
        public TestProcedureViewModel()
        {
            Blocks = new List<IBlock>()
            {
                new FixedTimeBlock(10),
             new QualityCheckProcedure(),
             new SchemaCreationProcedure(),

             new ArrangementProcedure(),
             new ClientCoordinationPrrocedure(),
             new DocumentationCoordinationProcedure(),
             new ElectricalSchemeSimulation(),
             new FormingDocumentationProcedure(),
             new PaperworkProcedure(),
             new SampleTestingProcedure(),
             new TracingProcedure()
            };
        }

        private List<IBlock> blocks;

        public List<IBlock> Blocks
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
