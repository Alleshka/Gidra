using System;
using System.Windows;

namespace GidraSIM
{
    /// <summary>
    /// Логика взаимодействия для CreateProcess.xaml
    /// </summary>
    public partial class CreateProcess : Window
    {
        public string NamePr;

        public CreateProcess(int number)
        {
            InitializeComponent();
            textBox_NameProcess.Text = "Процесс" + Convert.ToString(number);
        }

        private void button_Create_Click(object sender, RoutedEventArgs e)
        {
            NamePr = textBox_NameProcess.Text;
            this.Close();
        }

    }
}
