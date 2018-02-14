using System.Windows;
using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Procedures;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestProcedureSelectionDialog.xaml
    /// </summary>
    public partial class ProcedureSelectionDialog : Window
    {
        public ProcedureSelectionDialog()
        {
            InitializeComponent();
            listBox1.Items.Add(new AndBlock(2));
            listBox1.Items.Add(new DuplicateOutputsBlock(2));
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
            listBox1.Items.Add(new Assembling());
            listBox1.Items.Add(new Geometry2D());
            listBox1.Items.Add(new KDT());
            listBox1.Items.Add(new KinematicСalculations());
            listBox1.Items.Add(new StrengthСalculations());

            this.button.Focus();
            listBox1.SelectedIndex = 0;
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
