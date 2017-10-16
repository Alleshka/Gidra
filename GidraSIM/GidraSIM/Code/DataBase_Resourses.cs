using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using CommonData;
using GidraSIM.AdmSet;z

namespace GidraSIM
{
    public class DataBase_Resourses
    { 
        DataGrid dataGrid;               //текущий dataGrid, куда отображать таблицу
        int table;                       //номер текущей таблицы
        Window window;                   //текущее окно

        public DataBase_Resourses(DataGrid current_dataGrid, int number_table, Window current_window)
        {
            dataGrid = current_dataGrid;
            table = number_table;
            window = current_window;
        }

        public DataBase_Resourses()
        {
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
            ResourcesEntities data_base = new ResourcesEntities(SettingsReader.ResourcesConnectionString);
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
            try
            {
                switch (table)
                {
                    case 0:  //Workers
                        {
                            ResourcesEntities db = new ResourcesEntities(SettingsReader.ResourcesConnectionString);
                            db.Workers.AddObject(new Workers()
                            {
                                FIO = ((Workers)dataGrid.SelectedItem).FIO,
                                Position = ((Workers)dataGrid.SelectedItem).Position,
                                Qualification = ((Workers)dataGrid.SelectedItem).Qualification
                            });
                            db.SaveChanges();
                            break;
                        }
                    case 1: //CAD-systems
                        {
                            ResourcesEntities db = new ResourcesEntities(SettingsReader.ResourcesConnectionString);
                            db.CAD_Systems.AddObject(new CAD_Systems()
                            {
                                Name = ((CAD_Systems)dataGrid.SelectedItem).Name,
                                License_form = ((CAD_Systems)dataGrid.SelectedItem).License_form,
                                License_status = ((CAD_Systems)dataGrid.SelectedItem).License_status
                            });
                            db.SaveChanges();
                            break;
                        }
                    case 2: // Technical_Support
                        {
                            ResourcesEntities db = new ResourcesEntities(SettingsReader.ResourcesConnectionString);
                            db.Technical_Support.AddObject(new Technical_Support()
                            {
                                Name = ((Technical_Support)dataGrid.SelectedItem).Name,
                                Processor = ((Technical_Support)dataGrid.SelectedItem).Processor,
                                Processor_Memory = ((Technical_Support)dataGrid.SelectedItem).Processor_Memory,
                                Diagonal = ((Technical_Support)dataGrid.SelectedItem).Diagonal,
                                Video_card_Memory = ((Technical_Support)dataGrid.SelectedItem).Video_card_Memory
                            });
                            db.SaveChanges();
                            break;
                        }
                    case 3: // Methodological_Support
                        {
                            ResourcesEntities db = new ResourcesEntities(SettingsReader.ResourcesConnectionString);
                            db.Methodological_Support.AddObject(new Methodological_Support()
                            {
                                Doc_type = ((Methodological_Support)dataGrid.SelectedItem).Doc_type,
                                Multi_client_use = ((Methodological_Support)dataGrid.SelectedItem).Multi_client_use
                            });
                            db.SaveChanges();
                            break;
                        }
                }
                MessageBox.Show("Добавлено");
                RefreshForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //удаляем выделенную строку из таблицы----------------------------------------------------
        public void Delete_row()
        {
            try
            {
                ResourcesEntities db = new ResourcesEntities(SettingsReader.ResourcesConnectionString);
                int id_delete;

                switch (table) //выбираем таблицу для редактирования
                {
                    case 0:
                        {

                            id_delete = ((Workers)dataGrid.SelectedItem).worker_id;  //берем id выделенной строки
                            db.DeleteObject(db.Workers.Where(j => j.worker_id == id_delete).First());
                            break;
                        }
                    case 1:
                        {
                            id_delete = ((CAD_Systems)dataGrid.SelectedItem).cad_id;
                            db.DeleteObject(db.CAD_Systems.Where(j => j.cad_id == id_delete).First());
                            break;
                        }
                    case 2:
                        {
                            id_delete = ((Technical_Support)dataGrid.SelectedItem).tech_supp_id;
                            db.DeleteObject(db.Technical_Support.Where(j => j.tech_supp_id == id_delete).First());
                            break;
                        }
                    case 3:
                        {
                            id_delete = ((Methodological_Support)dataGrid.SelectedItem).method_supp_id;
                            db.DeleteObject(db.Methodological_Support.Where(j => j.method_supp_id == id_delete).First());
                            break;
                        }
                }
                db.SaveChanges();
                MessageBox.Show("Строка успешно удалена из таблицы");
                RefreshForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //обновление строки--------------------------------------------------------------------------------------------
        public void Update_row()
        {
            try
            {
                ResourcesEntities db = new ResourcesEntities(SettingsReader.ResourcesConnectionString);
                int id_update;

                switch (table) //выбираем таблицу для редактирования
                {
                    case 0:
                        {

                            id_update = ((Workers)dataGrid.SelectedItem).worker_id;  //берем id выделенной строки
                            Workers workers = db.Workers.Where(j => j.worker_id == id_update).First();
                            workers.FIO = ((Workers)dataGrid.SelectedItem).FIO;
                            workers.Position = ((Workers)dataGrid.SelectedItem).Position;
                            workers.Qualification = ((Workers)dataGrid.SelectedItem).Qualification;
                            db.SaveChanges();
                            break;
                        }
                    case 1:
                        {
                            id_update = ((CAD_Systems)dataGrid.SelectedItem).cad_id;  //берем id выделенной строки
                            CAD_Systems systems = db.CAD_Systems.Where(j => j.cad_id == id_update).First();
                            systems.Name = ((CAD_Systems)dataGrid.SelectedItem).Name;
                            systems.License_form = ((CAD_Systems)dataGrid.SelectedItem).License_form;
                            systems.License_status = ((CAD_Systems)dataGrid.SelectedItem).License_status;
                            db.SaveChanges();
                            break;
                        }
                    case 2:
                        {
                            id_update = ((Technical_Support)dataGrid.SelectedItem).tech_supp_id;  //берем id выделенной строки
                            Technical_Support technical_Support = db.Technical_Support.Where(j => j.tech_supp_id == id_update).First();
                            technical_Support.Name = ((Technical_Support)dataGrid.SelectedItem).Name;
                            technical_Support.Processor = ((Technical_Support)dataGrid.SelectedItem).Processor;
                            technical_Support.Processor_Memory = ((Technical_Support)dataGrid.SelectedItem).Processor_Memory;
                            technical_Support.Diagonal = ((Technical_Support)dataGrid.SelectedItem).Diagonal;
                            technical_Support.Video_card_Memory = ((Technical_Support)dataGrid.SelectedItem).Video_card_Memory;
                            db.SaveChanges();
                            break;
                        }
                    case 3:
                        {
                            id_update = ((Methodological_Support)dataGrid.SelectedItem).method_supp_id;  //берем id выделенной строки
                            Methodological_Support methodological_Support = db.Methodological_Support.Where(j => j.method_supp_id == id_update).First();
                            methodological_Support.Doc_type = ((Methodological_Support)dataGrid.SelectedItem).Doc_type;
                            methodological_Support.Multi_client_use = ((Methodological_Support)dataGrid.SelectedItem).Multi_client_use;
                            db.SaveChanges();
                            break;
                        }
                }
                MessageBox.Show("Строка успешно обновлена");
                RefreshForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //берем запись из таблицы--------------------------------------------------------------------------------------
        public List<string> GetInfoRow(ResourceTypes type, int id)
        {
            ResourcesEntities db = new ResourcesEntities(SettingsReader.ResourcesConnectionString);
            List<string> resultRow = new List<string>();
            switch (type)
            {
                case ResourceTypes.WORKER:
                    {
                        if (db.Workers.Any(j => j.worker_id == id))
                        {
                            Workers workers = db.Workers.Where(j => j.worker_id == id).First();
                            resultRow.Add(workers.FIO);
                            resultRow.Add(workers.Position);
                            resultRow.Add(workers.Qualification);
                        }
                        break;
                    }
                case ResourceTypes.CAD_SYSTEM:
                    {
                        if (db.CAD_Systems.Any(j => j.cad_id == id))
                        {
                            CAD_Systems cad = db.CAD_Systems.Where(j => j.cad_id == id).First();
                            resultRow.Add(cad.Name);
                            resultRow.Add(cad.License_form);
                            resultRow.Add(cad.License_status);
                        }
                        break;
                    }
                case ResourceTypes.TECHNICAL_SUPPORT:
                    {
                        if (db.Technical_Support.Any(j => j.tech_supp_id == id))
                        {
                            Technical_Support technical = db.Technical_Support.Where(j => j.tech_supp_id == id).First();
                            resultRow.Add(technical.Name);
                            resultRow.Add(technical.Processor);
                            resultRow.Add(technical.Processor_Memory);
                            resultRow.Add(technical.Diagonal);
                            resultRow.Add(technical.Video_card_Memory);
                        }
                        break;
                    }
                case ResourceTypes.METHODOLOGICAL_SUPPORT:
                    {
                        if (db.Methodological_Support.Any(j => j.method_supp_id == id))
                        {
                            Methodological_Support methodological = db.Methodological_Support.Where(j => j.method_supp_id == id).First();
                            resultRow.Add(methodological.Doc_type);
                            resultRow.Add(methodological.Multi_client_use);
                        }
                        break;
                    }
            }
            return resultRow;
        }
        //берем первый столбец из таблицы - имя------------------------------------------------------
        public string GetRowName(ResourceTypes type, int id)
        {
            ResourcesEntities db = new ResourcesEntities(SettingsReader.ResourcesConnectionString);
            string name_from_base = null;
            switch (type)
            {
                case ResourceTypes.WORKER:
                    {
                        Workers worker = db.Workers.Where(x => x.worker_id == id).First();
                        name_from_base = worker.FIO;
                        break;
                    }
                case ResourceTypes.CAD_SYSTEM:
                    {
                        CAD_Systems cad = db.CAD_Systems.Where(x => x.cad_id == id).First();
                        name_from_base = cad.Name;
                        break;
                    }
                case ResourceTypes.TECHNICAL_SUPPORT:
                    {
                        Technical_Support technical = db.Technical_Support.Where(x => x.tech_supp_id == id).First();
                        name_from_base = technical.Name;
                        break;
                    }
                case ResourceTypes.METHODOLOGICAL_SUPPORT:
                    {
                        Methodological_Support meh = db.Methodological_Support.Where(x => x.method_supp_id == id).First();
                        name_from_base = meh.Doc_type;
                        break;
                    }
            }
            return name_from_base;
        }

        private void RefreshForm()
        {
            //финт ушами
            window.Close();
            RedactTable_ResoursesDB again = new RedactTable_ResoursesDB(table);
            again.ShowDialog();
        }
    }
}
