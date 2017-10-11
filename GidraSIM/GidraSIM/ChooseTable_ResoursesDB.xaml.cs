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
    public partial class ChooseTable_ResoursesDB : Window
    {
        public int what_table;

        public ChooseTable_ResoursesDB()
        {
            InitializeComponent();
            what_table = -1;
        }

        private void button_Choose_Click(object sender, RoutedEventArgs e)  //выбор таблицы
        {
            if ((bool)radioButton_Workers.IsChecked)
                what_table = 0;
            else if ((bool)radioButton_CAD.IsChecked)
                what_table = 1;
            else if ((bool)radioButton_Tech.IsChecked)
                what_table = 2;
            else if ((bool)radioButton_Method.IsChecked)
                what_table = 3;

            if (what_table != -1)             //если ничего не выбрано
            {
                RedactTable_ResoursesDB window = new RedactTable_ResoursesDB(what_table);
                this.Close();
                window.ShowDialog();
            }
            else
                MessageBox.Show("Вы не выбрали таблицу", "Так что же делать?");
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)  //возвращение в исходное окно
        {
            this.Close();
        }
    }
}
