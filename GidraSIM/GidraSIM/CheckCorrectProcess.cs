using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonData;

namespace GidraSIM
{
    public class CheckCorrectProcess
    {
        Project project;   //текущий проект
        Process_ process;  //процесс для проверки
        Messages messages;          //класс вывода сообщений
        int number_process;

        public CheckCorrectProcess(Project project_to_check, int number_to_check, Messages new_messages)
        {
            project = project_to_check;
            process = project.Processes[number_to_check];
            messages = new_messages;
            number_process = number_to_check;
        }

        //проверка, все ли соединено между собой
        public bool CheckRoute()
        {
            for (int i = 0; i < process.Procedures.Count; i++)
            {
                if (process.Procedures[i].Left_Neibour.type == ObjectTypes.NO_OBJECT)
                    return false;
                else if (process.Procedures[i].Right_Neibour.type == ObjectTypes.NO_OBJECT)
                    return false;
            }
            return true;
        }

        //проверка, у всех ли процедур есть ресурсы
        public bool CheckProceduresOnResources()
        {
            for (int i = 0; i < process.Procedures.Count; i++)
                if (process.Procedures[i].Resources.Count == 0)
                    return false;
            return true;
        }

        //создание структуры процесса
        public void CreateStruct()
        {
            //построение структуры процесса
            process.StructProcess.RemoveRange(1, process.StructProcess.Count - 2);//очистка предыдущего построения
         //поиск первой процедуры----
           //идем по списку project.images_in_tabItem_in_tabItem со второго блока (т к первый и второй блоки - это начало и конец)
            for (int i = 2; i < project.Processes[number_process].images_in_tabItem.Count; i++)
                if (project.Processes[number_process].images_in_tabItem[i].object_of_block.Type != ObjectTypes.RESOURCE)//если блок - не ресурс, смотрим его
                    //если нашли ту, которая связана с началом, она должна быть первой процедурой в структуре процесса
                    switch (project.Processes[number_process].images_in_tabItem[i].object_of_block.Type)
                    {
                        case ObjectTypes.PROCEDURE:
                            if (process.Procedures[project.Processes[number_process].images_in_tabItem[i].object_of_block.number].Left_Neibour.type == ObjectTypes.BEGIN ||
                                process.Procedures[project.Processes[number_process].images_in_tabItem[i].object_of_block.number].Right_Neibour.type == ObjectTypes.BEGIN)
                            {
                                process.StructProcess.Insert(1, project.Processes[number_process].images_in_tabItem[i].object_of_block); //вставили первую процедуру
                                break;
                            }
                            break;
                        case ObjectTypes.SUBPROCESS:
                            if (process.SubProcesses[project.Processes[number_process].images_in_tabItem[i].object_of_block.number].Left_Neibour.type == ObjectTypes.BEGIN ||
                                process.SubProcesses[project.Processes[number_process].images_in_tabItem[i].object_of_block.number].Right_Neibour.type == ObjectTypes.BEGIN)
                            {
                                process.StructProcess.Insert(1, project.Processes[number_process].images_in_tabItem[i].object_of_block); //вставили первую процедуру
                                break;
                            }
                            break;
                    }
        //ищем остальные процедуры----
            bool block_in_struct = false;//флаг для наличия блока в структуре
            for (int i = 2; i < project.Processes[number_process].images_in_tabItem.Count; i++)
            {
                int num_curr_proced = process.StructProcess[process.StructProcess.Count - 2].number;//берем последнюю добавленную процедуру
                ObjectTypes curr_type = process.StructProcess[process.StructProcess.Count - 2].Type; //берем текущий тип
                for (int j = 2; j < project.Processes[number_process].images_in_tabItem.Count; j++)//идем от двух, т.к. 0 и 1 - начало и конец
                    {
                        block_in_struct = false;
                        if (project.Processes[number_process].images_in_tabItem[j].object_of_block.Type != ObjectTypes.RESOURCE)//если блок - не ресурс, смотрим его
                            switch (project.Processes[number_process].images_in_tabItem[j].object_of_block.Type)
                            {
                                case ObjectTypes.PROCEDURE:
                                        if (process.Procedures[project.Processes[number_process].images_in_tabItem[j].object_of_block.number].Left_Neibour.number == num_curr_proced
                                         || process.Procedures[project.Processes[number_process].images_in_tabItem[j].object_of_block.number].Right_Neibour.number == num_curr_proced)
                                            if (process.Procedures[project.Processes[number_process].images_in_tabItem[j].object_of_block.number].Left_Neibour.type == curr_type//т.к. номера оденаковыев разных списках, сравниваем еще и типы
                                            || process.Procedures[project.Processes[number_process].images_in_tabItem[j].object_of_block.number].Right_Neibour.type == curr_type)   
                                            {
                                                for (int k = 0; k < process.StructProcess.Count; k++)//проверяем, есть ли уже этот блок в структуре
                                                    if (process.StructProcess[k].number == project.Processes[number_process].images_in_tabItem[j].object_of_block.number)//проверка по номеру
                                                        if (process.StructProcess[k].Type == project.Processes[number_process].images_in_tabItem[j].object_of_block.Type)//проверка по типу
                                                            block_in_struct = true;//такой блок уже есть в структуре

                                                if (!block_in_struct)//добавляем, если такого блока еще нет с структуре
                                                {
                                                    process.StructProcess.Insert(process.StructProcess.Count - 1, project.Processes[number_process].images_in_tabItem[j].object_of_block); //вставили первую процедуру
                                                    break;
                                                }
                                            }
                                    break;
                                case ObjectTypes.SUBPROCESS:
                                      if (process.SubProcesses[project.Processes[number_process].images_in_tabItem[j].object_of_block.number].Left_Neibour.number == num_curr_proced
                                         || process.SubProcesses[project.Processes[number_process].images_in_tabItem[j].object_of_block.number].Right_Neibour.number == num_curr_proced)
                                          if (process.SubProcesses[project.Processes[number_process].images_in_tabItem[j].object_of_block.number].Left_Neibour.type == curr_type//т.к. номера оденаковыев разных списках, сравниваем еще и типы
                                              || process.SubProcesses[project.Processes[number_process].images_in_tabItem[j].object_of_block.number].Right_Neibour.type == curr_type)     
                                          {
                                                for (int k = 0; k < process.StructProcess.Count; k++)//проверяем, есть ли уже этот блок в структуре
                                                    if (process.StructProcess[k].number == project.Processes[number_process].images_in_tabItem[j].object_of_block.number)
                                                        if (process.StructProcess[k].Type == project.Processes[number_process].images_in_tabItem[j].object_of_block.Type)//проверка по типу
                                                            block_in_struct = true;//такой блок уже есть в структуре

                                                if (!block_in_struct)//добавляем, если такого блока еще нет с структуре
                                                {
                                                    process.StructProcess.Insert(process.StructProcess.Count - 1, project.Processes[number_process].images_in_tabItem[j].object_of_block); //вставили первую процедуру
                                                    break;
                                                }
                                           }
                                    break;
                            }
                    }
            }
        }

    }
}