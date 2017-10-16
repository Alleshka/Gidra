using System;
using System.Windows;
using GidraSIM.AdmSet;

namespace GidraSIM
{
    public class DataBase_ModelingSession
    {
        Project project;                   //текущий проект
        Process_ modeled_process;          //текущий процесс, который смоделировали

        public DataBase_ModelingSession(Project new_project, int number_process_to_base)
        {       
            project = new_project;
            modeled_process = project.Processes[number_process_to_base]; //берем смоделированный процесс
        }

        public DataBase_ModelingSession()
        {
        }

        //сохраняем все в базу
        public void SaveToBase(int number_modeled)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //сохраняем с таблицу очередной процесс
        public bool SaveToBaseProcess(int number_saving) //number_saving - номер текущего процесса, который сохраняем в базу
        {
            try
            {
                ModelingSessionEntities db = new ModelingSessionEntities(SettingsReader.ModelingSessionConnectionString);

                ConvertTimeUnits convertTime = new ConvertTimeUnits();


                Modeling_Process modeling = new Modeling_Process()
                {
                    Name_process = project.Processes[number_saving].Name,
                    Procedures_number = project.Processes[number_saving].Procedures.Count, //общее число процедур
                    Subprocesses_number = project.Processes[number_saving].SubProcesses.Count, //число вложенных процессов
                    AllTime = project.Processes[number_saving].Time_in_format,  //общее время выполнения
                };
                if (modeled_process.IsSub != -2) //вообще-то -1, но пока так
                {
                    modeling.Parent_process_id = project.Processes[number_saving].IsSub.ToString();
                }
                else
                {
                    modeling.Parent_process_id = "не вложен";
                }
                if (project.Processes[number_saving].Time_accidents_in_days > 0)
                {
                    modeling.Accidents = project.Processes[number_saving].Time_accidents_in_format; //из него - задержки
                }
                else
                {
                    modeling.Accidents = "нет";
                }

                db.Modeling_Process.AddObject(modeling);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        
        //сохраняем процедуры процесса(из его списка процедур)
        public void SaveToBaseProcedures(int number_saving) //number_saving - номер текущего процесса, который сохраняем в базу
        {
            try
            {
                ModelingSessionEntities db = new ModelingSessionEntities(SettingsReader.ModelingSessionConnectionString);
                ConvertTimeUnits convertTime = new ConvertTimeUnits();
                for (int i = 0; i < project.Processes[number_saving].Procedures.Count; i++)
                {
                    ModelingProcedures modeling = new ModelingProcedures()
                    {
                        Name_process = project.Processes[number_saving].Name, //имя процесса
                        Name_procedure = Convert.ToString(project.Processes[number_saving].Procedures[i].Name), //имя процедуры
                        Calculated_Time = project.Processes[number_saving].Procedures[i].Time_in_format
                    };

                    if (project.Processes[number_saving].Procedures[i].is_common)
                    {
                        modeling.Type_procedure = project.Processes[number_saving].Procedures[i].common_type; //тип процедуры
                    }
                    else
                    {
                        modeling.Type_procedure = "с фиксированным временем";  //если нет типа (фикс.время)      
                    }
                    db.ModelingProcedures.AddObject(modeling);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ClearTables()
        {
            try
            {
                ModelingSessionEntities db = new ModelingSessionEntities(SettingsReader.ModelingSessionConnectionString);
                db.ExecuteStoreCommand("TRUNCATE TABLE Modeling_Process");
                db.ExecuteStoreCommand("TRUNCATE TABLE ModelingProcedures");
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
