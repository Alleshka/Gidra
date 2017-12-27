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
    /// Логика взаимодействия для TestComplexitySelectionDialog.xaml
    /// </summary>
    public partial class TestComplexitySelectionDialog : Window
    {
        public TestComplexitySelectionDialog()
        {
            InitializeComponent();
            
            listBox1.Items.Add(new ListBoxItem() { Content = "Очень легкая"} );//0
            listBox1.Items.Add(new ListBoxItem() { Content = "Легкая" } );//1
            listBox1.Items.Add(new ListBoxItem() { Content = "Средняя " } );//2
            listBox1.Items.Add(new ListBoxItem() { Content = "Сложная" } );//3
            listBox1.Items.Add(new ListBoxItem() { Content = "Оень сложная" } );//4

            listBox1.SelectedIndex = 0;
            this.button.Focus();
        }

        public double Complexity { get; set; }
        public double Step { get; set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //SelectedBlock = listBox1.SelectedItem as ProcedureWPF;
            //listBox1.Items.Remove(listBox1.SelectedItem);
            switch(listBox1.SelectedIndex)
            {
                case 0:
                    Complexity = 0.2;
                    break;
                case 1:
                    Complexity = 2;
                    break;
                case 2:
                    Complexity = 4;
                    break;
                case 3:
                    Complexity = 7;
                    break;
                case 4:
                    Complexity = 10;
                    break;
            };
            Step = double.Parse(stepTextBox.Text);

            this.DialogResult = true;
        }
    }
}
