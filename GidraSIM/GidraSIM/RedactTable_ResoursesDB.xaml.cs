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
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data;

namespace GidraSIM
{
    public partial class RedactTable_ResoursesDB : Window
    {
        DataBase_Resourses db_Action;

        public RedactTable_ResoursesDB(int number)
        {
            InitializeComponent();
            db_Action = new DataBase_Resourses(dataGrid1, number, this);

            db_Action.LoadTable(); //создаем таблицу
            db_Action.ShowTable(); //загружаем содержимое таблицы
        }

        //добавление строки
        private void button_Add_Click(object sender, RoutedEventArgs e)  //вставить строку
        {
            if (dataGrid1.SelectedItem != null)
                db_Action.Add_newRow();
            else
                MessageBox.Show("Не выделена ни одна строка", "Так не получится");
        }

        //удалние строки
        private void button_Delete_Click(object sender, RoutedEventArgs e)  //удалить строку
        {
            if (dataGrid1.SelectedItem != null)
                db_Action.Delete_row();
            else
                MessageBox.Show("Не выделена ни одна строка", "Так не получится");
        }

        //обновление строки
        private void button_Redact_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
                db_Action.Update_row();
            else
                MessageBox.Show("Не выделена ни одна строка", "Так не получится");
        }

        //возврат на выбор таблицы-------------------------------------------------------------
        private void button_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ChooseTable_ResoursesDB back = new ChooseTable_ResoursesDB();
            back.ShowDialog();
        }
    }
}
