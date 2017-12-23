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
using GidraSIM.GUI.Core.BlocksWPF.ViewModels.Procedures;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestProcedureSelectionDialog.xaml
    /// </summary>
    public partial class TestProcedureSelectionDialog : Window
    {
        public TestProcedureSelectionDialog(Point position)
        {
            InitializeComponent();
            listBox1.Items.Add( new FixedTimeBlockViewModel(position,"Блок фикс времени"));
            listBox1.Items.Add(new QualityCheckProcedureViewModel(position, "Проверка качества"));
            listBox1.Items.Add(new SchemaCreationProcedureViewModel(position, "Проектирование эл схемы"));

            listBox1.Items.Add(new ArrangementProcedureViewModel(position, "Компоновка"));
            listBox1.Items.Add(new ClientCoordinationPrrocedureViewModel(position, "Согласование доков с заказчиком"));
            listBox1.Items.Add(new DocumentationCoordinationProcedureViewModel(position, "Согласование с нормоконтролем"));
            listBox1.Items.Add(new ElectricalSchemeSimulationViewModel(position, "Моделирование эл схемы"));
            listBox1.Items.Add(new FormingDocumentationProcedureViewModel(position, "Формирование документации"));
            listBox1.Items.Add(new PaperworkProcedureViewModel(position, "Оформление документации"));
            listBox1.Items.Add(new SampleTestingProcedureViewModel(position, "Тестирование образца"));
            listBox1.Items.Add(new TracingProcedureViewModel(position, "Трассировка"));

            listBox1.SelectedIndex = 0;
            this.button.Focus();
        }

        public ProcedureWPF SelectedBlock { get; private set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SelectedBlock = listBox1.SelectedItem as ProcedureWPF;
            listBox1.Items.Remove(listBox1.SelectedItem);
            this.DialogResult = true;
        }
    }
}
