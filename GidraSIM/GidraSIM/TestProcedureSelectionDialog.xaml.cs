using System.Windows;
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
            InitializeComponent();
            listBox1.Items.Add(new FixedTimeBlock(10));
            listBox1.Items.Add(new QualityCheckProcedure());
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
