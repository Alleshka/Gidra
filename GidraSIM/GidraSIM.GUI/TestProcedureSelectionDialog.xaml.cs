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

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestProcedureSelectionDialog.xaml
    /// </summary>
    public partial class TestProcedureSelectionDialog : Window
    {
        public TestProcedureSelectionDialog()
        {
            InitializeComponent();
            listBox1.Items.Add(new FixedTimeBlock(10));
            listBox1.Items.Add(new QualityCheckProcedure());
            listBox1.Items.Add(new SchemaCreationProcedure());

            listBox1.Items.Add(new ArrangementProcedure());
            listBox1.Items.Add(new ClientCoordinationPrrocedure());
            listBox1.Items.Add(new DocumentationCoordinationProcedure());
            listBox1.Items.Add(new ElectricalSchemeSimulation());
            listBox1.Items.Add(new FormingDocumentationProcedure());
            listBox1.Items.Add(new PaperworkProcedure());
            listBox1.Items.Add(new SampleTestingProcedure());
            listBox1.Items.Add(new TracingProcedure());

            listBox1.SelectedIndex = 0;
            //this.button.Focus();
        }

        public IBlock SelectedBlock { get; private set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SelectedBlock = listBox1.SelectedItem as IBlock;
            listBox1.Items.Remove(listBox1.SelectedItem);
            this.DialogResult = true;
        }
    }
}
