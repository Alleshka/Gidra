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
using CommonData;

namespace GidraSIM
{
    public class DataBase_Resourses
    {
        ResourcesEntities data_base;  //объявляем базу данных
        DataGrid dataGrid;               //текущий dataGrid, куда отображать таблицу
        int table;                       //номер текущей таблицы
        Window window;                   //текущее окно
        SqlServer sqlServer;           //класс, возвращающий имя локального сервера

        public DataBase_Resourses(DataGrid current_dataGrid, int number_table, Window current_window)
        {
            dataGrid = current_dataGrid;
            table = number_table;
            data_base = new ResourcesEntities();
            sqlServer = new SqlServer();
            window = current_window;
        }

        public DataBase_Resourses()
        {
            data_base = new ResourcesEntities();
            sqlServer = new SqlServer();
        }

        //создаем соединение с базой данных
        public SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = @"Data Source=" + sqlServer.GetServerName() + ";Initial Catalog=Resources;Integrated Security=True";
            connection.Open();

            return connection;
        }

        //настраиваем dataGrid----------------------------------------------------------------------------------------------------
        public void LoadTable()
        {
            switch (table)
            {
                case 0:  //Workers
                    {
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "ФИО", Binding = new Binding("FIO") });
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Должность", Binding = new Binding("Position") });
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Квалификация", Binding = new Binding("Qualification") });
                    }
                    break;
                case 1: //CAD_Systems
                    {
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Имя", Binding = new Binding("Name") });
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Форма лиицензии", Binding = new Binding("License_form") });
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Статус лицензии", Binding = new Binding("License_status") }); ;
                    }
                    break;
                case 2:  //Technical_Support
                    {
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Условное имя компьютера", Binding = new Binding("Name") });
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Частота процессора, Ггц", Binding = new Binding("Processor") });
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Память процессора, Гб", Binding = new Binding("Processor_Memory") });
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Диагональ монитора, дюйм", Binding = new Binding("Diagonal") });
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Память видеокарты, Мб", Binding = new Binding("Video_card_Memory") });
                    }
                    break;
                case 3:  //Methodological_Support
                    {
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Тип", Binding = new Binding("Doc_type") });
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Multiuse", Binding = new Binding("Multi_client_use") });
                    }
                    break;
            }
        }

        //отображаем содержимое таблицы-------------------------------------------------------------------------------------------
        public void ShowTable()
        {
            switch (table)
            {
                case 0:
                    {
                        dataGrid.ItemsSource = data_base.Workers;  //исполнители
                        if (data_base.Workers.Count() == 0)
                            MessageBox.Show("Таблица пуста", "Увы");
                    }
                    break;
                case 1:
                    {
                        dataGrid.ItemsSource = data_base.CAD_Systems;  //САПР-системы
                        if (data_base.CAD_Systems.Count() == 0)
                            MessageBox.Show("Таблица пуста", "Увы");
                    }
                    break;
                case 2:
                    {
                        dataGrid.ItemsSource = data_base.Technical_Support;  //технологич. обеспеч.
                        if (data_base.Technical_Support.Count() == 0)
                            MessageBox.Show("Таблица пуста", "Увы");
                    }
                    break;
                case 3:
                    {
                        dataGrid.ItemsSource = data_base.Methodological_Support;  //методлогич. обеспеч.
                        if (data_base.Methodological_Support.Count() == 0)
                            MessageBox.Show("Таблица пуста", "Увы");
                    }
                    break;
            }
        }

        //добавляем выделенную строку в таблицу-----------------------------------------------------------------------------------------------
        public void Add_newRow()
        {
            string sql;
            SqlCommand add;
            SqlConnection connection = CreateConnection(); //устанавливаем соединение с базой

            switch (table)
            {
                case 0:  //Workers
                    {
                        //пишем запрос
                        sql = string.Format("INSERT INTO Workers" +
                            "(FIO, Position, Qualification) Values(@FIO, @Position, @Qualification)");
                        //создаем команду с параметрами
                        add = new SqlCommand(sql, connection);
                        add.Parameters.AddWithValue("@FIO", ((Workers)dataGrid.SelectedItem).FIO);
                        add.Parameters.AddWithValue("@Position", ((Workers)dataGrid.SelectedItem).Position);
                        add.Parameters.AddWithValue("@Qualification", ((Workers)dataGrid.SelectedItem).Qualification);
                        //пытаемся выполнить запрос с текущим соединением
                        try
                        {
                            add.ExecuteNonQuery();
                            MessageBox.Show("Строка успешно добавлена в таблицу", "Все путем");
                        }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
                case 1:  //CAD-systems
                    {
                        sql = string.Format("INSERT INTO CAD_Systems" +
                            "(Name, License_form, License_status)" +
                            "Values(@Name,  @License_form, @License_status)");
                        add = new SqlCommand(sql, connection);
                        add.Parameters.AddWithValue("@Name", ((CAD_Systems)dataGrid.SelectedItem).Name);
                        add.Parameters.AddWithValue("@License_form", ((CAD_Systems)dataGrid.SelectedItem).License_form);
                        add.Parameters.AddWithValue("@License_status", ((CAD_Systems)dataGrid.SelectedItem).License_status);

                        try
                        {
                            add.ExecuteNonQuery();
                            MessageBox.Show("Строка успешно добавлена в таблицу", "Все путем");
                        }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
                case 2:
                    {
                        sql = string.Format("INSERT INTO Technical_Support" +
                            "(Name, Processor, Processor_Memory, Diagonal, Video_card_Memory) " +
                            "Values(@Name, @Processor, @Processor_Memory, @Diagonal, @Video_card_Memory)");
                        add = new SqlCommand(sql, connection);
                        add.Parameters.AddWithValue("@Name", ((Technical_Support)dataGrid.SelectedItem).Name);
                        add.Parameters.AddWithValue("@Processor", ((Technical_Support)dataGrid.SelectedItem).Processor);
                        add.Parameters.AddWithValue("@Processor_Memory", ((Technical_Support)dataGrid.SelectedItem).Processor_Memory);
                        add.Parameters.AddWithValue("@Diagonal", ((Technical_Support)dataGrid.SelectedItem).Diagonal);
                        add.Parameters.AddWithValue("@Video_card_Memory", ((Technical_Support)dataGrid.SelectedItem).Video_card_Memory);

                        try
                        {
                            add.ExecuteNonQuery();
                            MessageBox.Show("Строка успешно добавлена в таблицу", "Все путем");
                        }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
                case 3:
                    {
                        sql = string.Format("INSERT INTO Methodological_Support" +
                            "(Doc_type, Multi_client_use) Values(@Doc_type, @Multi_client_use)");
                        add = new SqlCommand(sql, connection);
                        add.Parameters.AddWithValue("@Doc_type", ((Methodological_Support)dataGrid.SelectedItem).Doc_type);
                        add.Parameters.AddWithValue("@Multi_client_use", ((Methodological_Support)dataGrid.SelectedItem).Multi_client_use);

                        try
                        {
                            add.ExecuteNonQuery();
                            MessageBox.Show("Строка успешно добавлена в таблицу", "Все путем");
                        }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
            }
            connection.Close();  //завершаем соединение с базой
        }

        //удаляем выделенную строку из таблицы----------------------------------------------------
        public void Delete_row()
        {
            int id_delete;
            string sql;
            SqlCommand delete;
            SqlConnection connection = CreateConnection(); //устанавливаем соединение с базой

            switch (table) //выбираем таблицу для редактирования
            {
                case 0:
                    {
                        id_delete = ((Workers)dataGrid.SelectedItem).worker_id;  //берем id выделенной строки
                        sql = string.Format("DELETE FROM Workers WHERE worker_id = '{0}'", id_delete);  //создаем запрос
                        delete = new SqlCommand(sql, connection);  //создаем команду
                        //пытаемся выполнить запрос с установленным соединением
                        try { delete.ExecuteNonQuery(); }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
                case 1:
                    {
                        id_delete = ((CAD_Systems)dataGrid.SelectedItem).cad_id;
                        sql = string.Format("DELETE FROM CAD_Systems WHERE cad_id = '{0}'", id_delete);
                        delete = new SqlCommand(sql, connection);
                        try { delete.ExecuteNonQuery(); }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
                case 2:
                    {
                        id_delete = ((Technical_Support)dataGrid.SelectedItem).tech_supp_id;
                        sql = string.Format("DELETE FROM Technical_Support WHERE tech_supp_id = '{0}'", id_delete);
                        delete = new SqlCommand(sql, connection);
                        try { delete.ExecuteNonQuery(); }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
                case 3:
                    {
                        id_delete = ((Methodological_Support)dataGrid.SelectedItem).method_supp_id;
                        sql = string.Format("DELETE FROM Methodological_Support WHERE method_supp_id = '{0}'", id_delete);
                        delete = new SqlCommand(sql, connection);
                        try { delete.ExecuteNonQuery(); }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
            }
            MessageBox.Show("Строка успешно удалена из таблицы", "Все путем");

            connection.Close();  //завершаем соединение с базой   

            //финт ушами
            window.Close();
            RedactTable_ResoursesDB again = new RedactTable_ResoursesDB(table);
            again.ShowDialog();
        }

        //обновление строки--------------------------------------------------------------------------------------------
        public void Update_row()
        {
            int id_update;
            string sql;
            SqlCommand update;
            SqlConnection connection = CreateConnection(); //устанавливаем соединение с базой

            switch (table) //выбираем таблицу для редактирования
            {
                case 0:
                    {
                        id_update = ((Workers)dataGrid.SelectedItem).worker_id;  //берем id выделенной строки
                        sql = string.Format("UPDATE Workers SET FIO ='{0}', Position = '{1}', Qualification = '{2}' WHERE worker_id = '{3}'",
                            ((Workers)dataGrid.SelectedItem).FIO, ((Workers)dataGrid.SelectedItem).Position,
                            ((Workers)dataGrid.SelectedItem).Qualification, id_update);  //создаем запрос
                        update = new SqlCommand(sql, connection);  //создаем команду
                        //пытаемся выполнить запрос с установленным соединением
                        try { update.ExecuteNonQuery(); }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
                case 1:
                    {
                        id_update = ((CAD_Systems)dataGrid.SelectedItem).cad_id;  //берем id выделенной строки
                        sql = string.Format("UPDATE CAD_Systems SET Name ='{0}', License_form = '{1}', License_status = '{2}' WHERE cad_id = '{3}'",
                            ((CAD_Systems)dataGrid.SelectedItem).Name, ((CAD_Systems)dataGrid.SelectedItem).License_form,
                            ((CAD_Systems)dataGrid.SelectedItem).License_status, id_update);  //создаем запрос
                        update = new SqlCommand(sql, connection);  //создаем команду
                        //пытаемся выполнить запрос с установленным соединением
                        try { update.ExecuteNonQuery(); }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
                case 2:
                    {
                        id_update = ((Technical_Support)dataGrid.SelectedItem).tech_supp_id;  //берем id выделенной строки
                        sql = string.Format("UPDATE Technical_Support SET Name ='{0}', Processor = '{1}', Processor_Memory = '{2}'," +
                            "Diagonal = '{3}', Video_card_Memory = '{4}' WHERE tech_supp_id = '{5}'",
                            ((Technical_Support)dataGrid.SelectedItem).Name, ((Technical_Support)dataGrid.SelectedItem).Processor,
                            ((Technical_Support)dataGrid.SelectedItem).Processor_Memory, ((Technical_Support)dataGrid.SelectedItem).Diagonal,
                            ((Technical_Support)dataGrid.SelectedItem).Video_card_Memory, id_update);  //создаем запрос
                        update = new SqlCommand(sql, connection);  //создаем команду
                        //пытаемся выполнить запрос с установленным соединением
                        try { update.ExecuteNonQuery(); }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
                case 3:
                    {
                        id_update = ((Methodological_Support)dataGrid.SelectedItem).method_supp_id;  //берем id выделенной строки
                        sql = string.Format("UPDATE Methodological_Support SET Doc_type ='{0}', Multi_client_use = '{1}' WHERE method_supp_id = '{2}'",
                            ((Methodological_Support)dataGrid.SelectedItem).Doc_type,
                            ((Methodological_Support)dataGrid.SelectedItem).Multi_client_use, id_update);  //создаем запрос
                        update = new SqlCommand(sql, connection);  //создаем команду
                        //пытаемся выполнить запрос с установленным соединением
                        try { update.ExecuteNonQuery(); }
                        catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
                    }
                    break;
            }
            MessageBox.Show("Строка успешно обновлена", "Все путем");

            connection.Close();  //завершаем соединение с базой   

            //финт ушами
            window.Close();
            RedactTable_ResoursesDB again = new RedactTable_ResoursesDB(table);
            again.ShowDialog();
        }

        //берем запись из таблицы--------------------------------------------------------------------------------------
        public List<string> GetInfoRow(ResourceTypes type, int id)
        {
            SqlCommand select;
            SqlConnection connection = CreateConnection();
            string sql;
            switch (type)
            {
                case ResourceTypes.WORKER: sql = string.Format("SELECT * FROM Workers WHERE worker_id = '{0}'", id);
                    break;
                case ResourceTypes.CAD_SYSTEM: sql = string.Format("SELECT * FROM CAD_Systems WHERE cad_id = '{0}'", id);
                    break;
                case ResourceTypes.TECHNICAL_SUPPORT: sql = string.Format("SELECT * FROM Technical_Support WHERE tech_supp_id = '{0}'", id);
                    break;
                case ResourceTypes.METHODOLOGICAL_SUPPORT: sql = string.Format("SELECT * FROM Methodological_Support WHERE method_supp_id = '{0}'", id);
                    break;
                default: sql = "SELECT * FROM Workers";
                    break;
            }

            List<string> resultRow = new List<string>();
            select = new SqlCommand(sql, connection);
            try
            {
                SqlDataReader reader = select.ExecuteReader();

                while (reader.Read())
                {
                    resultRow.Add(reader.GetString(1));
                    resultRow.Add(reader.GetString(2));
                    if (type == ResourceTypes.WORKER || type == ResourceTypes.CAD_SYSTEM || type == ResourceTypes.TECHNICAL_SUPPORT)
                        resultRow.Add(reader.GetString(3));
                    if (type == ResourceTypes.TECHNICAL_SUPPORT)
                    {
                        resultRow.Add(reader.GetString(4));
                        resultRow.Add(reader.GetString(5));
                    }
                }
                reader.Close();
            }
            catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
            return resultRow;
        }

        //берем первый столбец из таблицы - имя------------------------------------------------------
        public string GetRowName(ResourceTypes type, int id)
        {
            SqlCommand select;
            SqlConnection connection = CreateConnection();
            string sql;
            switch (type)
            {
                case ResourceTypes.WORKER: sql = string.Format("SELECT * FROM Workers WHERE worker_id = '{0}'", id);
                    break;
                case ResourceTypes.CAD_SYSTEM: sql = string.Format("SELECT * FROM CAD_Systems WHERE cad_id = '{0}'", id);
                    break;
                case ResourceTypes.TECHNICAL_SUPPORT: sql = string.Format("SELECT * FROM Technical_Support WHERE tech_supp_id = '{0}'", id);
                    break;
                case ResourceTypes.METHODOLOGICAL_SUPPORT: sql = string.Format("SELECT * FROM Methodological_Support WHERE method_supp_id = '{0}'", id);
                    break;
                default: sql = "SELECT * FROM Workers";
                    break;
            }
            string name_from_base = null;
            select = new SqlCommand(sql, connection);
            try
            {
                SqlDataReader reader = select.ExecuteReader();
                while (reader.Read())
                    name_from_base = reader.GetString(1);
                reader.Close();
            }
            catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
            return name_from_base;
        }
    }
}
