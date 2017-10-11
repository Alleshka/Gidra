using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
