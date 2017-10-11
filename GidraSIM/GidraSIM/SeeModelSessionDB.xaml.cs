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
    /// Логика взаимодействия для SeeModelSessionDB.xaml
    /// </summary>
    public partial class SeeModelSessionDB : Window
    {
        DataBase_ModelingSession data_base;

        public SeeModelSessionDB()
        {
            InitializeComponent();
            data_base = new DataBase_ModelingSession();
        }

        private void ShowAll()
        {
            dataGrid_processes.Columns.Add(new DataGridTextColumn() { Header = "Имя процесса", Binding = new Binding("Name_process") });
            dataGrid_processes.Columns.Add(new DataGridTextColumn() { Header = "Родительский процесс", Binding = new Binding("Parent_process_id") });
            dataGrid_processes.Columns.Add(new DataGridTextColumn() { Header = "Время выполнения", Binding = new Binding("AllTime") });
            dataGrid_processes.Columns.Add(new DataGridTextColumn() { Header = "Временные задержки", Binding = new Binding("Accidents") });
            dataGrid_processes.Columns.Add(new DataGridTextColumn() { Header = "Число процедур", Binding = new Binding("Procedures_number") });
            dataGrid_processes.Columns.Add(new DataGridTextColumn() { Header = "Число вложенных процессов", Binding = new Binding("Subprocesses_number") });
        }

        private void button_ClearTables_Click(object sender, RoutedEventArgs e)
        {
             
        }
    }
}
