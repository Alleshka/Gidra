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
    public class DataBase_ModelingSession
    {
        ModelingSessionEntities data_base; //объявляем базу данных
        SqlServer sqlServer;               //класс, возвращающий имя локального сервера
        Project project;                   //текущий проект
        Process_ modeled_process;          //текущий процесс, который смоделировали

        public DataBase_ModelingSession(Project new_project, int number_process_to_base)
        {
            data_base = new ModelingSessionEntities();  //создаем базу
            sqlServer = new SqlServer();                
            project = new_project;
            modeled_process = project.Processes[number_process_to_base]; //берем смоделированный процесс
        }

        public DataBase_ModelingSession()
        {
            data_base = new ModelingSessionEntities();  //создаем базу
            sqlServer = new SqlServer();    
        }

        //сохраняем все в базу
        public void SaveToBase(int number_modeled)
        {
            if (SaveToBaseProcess(number_modeled)) //записываем в таблицу процесс,если успешно-пишем процедуры
            {
                SaveToBaseProcedures(number_modeled);  //записываем его процедуры
                if (modeled_process.SubProcesses.Count > 0) //если есть вложенные процессы
                    for (int j = 0; j < modeled_process.SubProcesses.Count; j++) //идем по вложенным и пишем их и их процедуры в базу
                        SaveToBase(modeled_process.SubProcesses[j].number_in_processes); //уопачки рекурсия!
            }
            MessageBox.Show("Результаты моделирования успешно записаны в базу данных", "Все хорошо");
        }

        //создаем соединение с базой данных
        public SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = @"Data Source=" + sqlServer.GetServerName() + ";Initial Catalog=ModelingSession;Integrated Security=True";
            connection.Open(); //открываем соединение

            return connection;
        }

        //сохраняем с таблицу очередной процесс
        public bool SaveToBaseProcess(int number_saving) //number_saving - номер текущего процесса, который сохраняем в базу
        {
            ConvertTimeUnits convertTime = new ConvertTimeUnits();
            string sql;
            SqlCommand add;
            SqlConnection connection = CreateConnection(); //устанавливаем соединение с базой//пишем запрос
            sql = string.Format("INSERT INTO Modeling_Process (Name_process, Parent_process_id, AllTime, Accidents, Procedures_number, Subprocesses_number)" +
                                "Values(@Name_process, @Parent_process_id, @AllTime, @Accidents, @Procedures_number, @Subprocesses_number)");
            //создаем команду с параметрами
            add = new SqlCommand(sql, connection);
            add.Parameters.AddWithValue("@Name_process", project.Processes[number_saving].Name); //имя процесса
            if (modeled_process.IsSub != -2) //вообще-то -1, но пока так
                add.Parameters.AddWithValue("@Parent_process_id", project.Processes[number_saving].IsSub); //куда он вложен
            else
                add.Parameters.AddWithValue("@Parent_process_id", "не вложен");  //никуда не вложен
            add.Parameters.AddWithValue("@AllTime", project.Processes[number_saving].Time_in_format);  //общее время выполнения
            if (project.Processes[number_saving].Time_accidents_in_days > 0)
                add.Parameters.AddWithValue("@Accidents", project.Processes[number_saving].Time_accidents_in_format); //из него - задержки
            else
                add.Parameters.AddWithValue("@Accidents", "нет");
            add.Parameters.AddWithValue("@Procedures_number", project.Processes[number_saving].Procedures.Count);  //общее число процедур
            add.Parameters.AddWithValue("@Subprocesses_number", project.Processes[number_saving].SubProcesses.Count);  //число вложенных процессов
            //пытаемся выполнить запрос с текущим соединением
            try
            {
                add.ExecuteNonQuery();
                return true;
            }
            catch (SqlException exe)
            {
                MessageBox.Show(exe.Message, "Ой!");
                return false;
            }
        }
        
        //сохраняем процедуры процесса(из его списка процедур)
        public void SaveToBaseProcedures(int number_saving) //number_saving - номер текущего процесса, который сохраняем в базу
        {
            ConvertTimeUnits convertTime = new ConvertTimeUnits();
            string sql;
            SqlCommand add;
            SqlConnection connection = CreateConnection(); //устанавливаем соединение с базой//пишем запрос
            for (int i = 0; i < project.Processes[number_saving].Procedures.Count; i++)
            {
                sql = string.Format("INSERT INTO ModelingProcedures (Name_process, Name_procedure, Type_procedure, Calculated_Time)" +
                                    "Values(@Name_process, @Name_procedure, @Type_procedure, @Calculated_Time)");
                //создаем команду с параметрами
                add = new SqlCommand(sql, connection);
                add.Parameters.AddWithValue("@Name_process", project.Processes[number_saving].Name); //имя процесса
                add.Parameters.AddWithValue("@Name_procedure", Convert.ToString(project.Processes[number_saving].Procedures[i].Name));  //имя процедуры
                if (project.Processes[number_saving].Procedures[i].is_common)
                    add.Parameters.AddWithValue("@Type_procedure", project.Processes[number_saving].Procedures[i].common_type); //тип процедуры
                else
                    add.Parameters.AddWithValue("@Type_procedure", "с фиксированным временем");  //если нет типа (фикс.время)               
                add.Parameters.AddWithValue("@Calculated_Time", project.Processes[number_saving].Procedures[i].Time_in_format); //время выполнения
                try  {  add.ExecuteNonQuery();  }
                catch (SqlException exe)  {  MessageBox.Show(exe.Message, "Ой!"); }
            }
        }

        public void ClearTables()
        {
            string sql;
            SqlCommand truncate;
            SqlConnection connection = CreateConnection(); //устанавливаем соединение с базой//пишем запрос

            sql = string.Format("TRUNCATE TABLE Modeling_Process");
            truncate = new SqlCommand(sql, connection);
            try { truncate.ExecuteNonQuery(); }
            catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }

            sql = string.Format("TRUNCATE TABLE ModelingProcedures");
            truncate = new SqlCommand(sql, connection);
            try { truncate.ExecuteNonQuery(); }
            catch (SqlException exe) { MessageBox.Show(exe.Message, "Ой!"); }
        }
    }
}
