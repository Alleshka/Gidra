using System.Windows;
using System.Windows.Controls;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestComplexitySelectionDialog.xaml
    /// </summary>
    public partial class ComplexitySelectionDialog : Window
    {
        public ComplexitySelectionDialog()
        {
            InitializeComponent();
            
            listBox1.Items.Add(new ListBoxItem() { Content = "Очень низкая"} );//0
            listBox1.Items.Add(new ListBoxItem() { Content = "Низкая" } );//1
            listBox1.Items.Add(new ListBoxItem() { Content = "Средняя " } );//2
            listBox1.Items.Add(new ListBoxItem() { Content = "Сложная" } );//3
            listBox1.Items.Add(new ListBoxItem() { Content = "Оень сложная" } );//4

            listBox1.SelectedIndex = 0;
            this.button.Focus();
        }

        public double Complexity { get; set; }
        public double Step { get; set; }
        public double MaxTime { get; set; }

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
            Step = double.Parse(stepTextBox.Text, System.Globalization.CultureInfo.InvariantCulture);
            MaxTime = double.Parse(summaryTimeTextBox.Text);

            this.DialogResult = true;
        }
    }
}
