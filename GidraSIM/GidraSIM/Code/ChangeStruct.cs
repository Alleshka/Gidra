using CommonData;
using System.Windows;
using System.Windows.Controls;

namespace GidraSIM
{
    public class ChangeStruct   //класс для изменения структуры блоков
    {
        Project project;
        DrawShema draw;
        int current_process;

        public ChangeStruct(ref Project new_project, ref DrawShema new_draw, int number_process)
        {
            draw = new_draw;
            project = new_project;
            current_process = number_process;
        }

        public void ChangeNumberProcess(int number_process)
         {
             current_process = number_process;
         }

//думаем, какие у кого соседи
        public void SetStructNeibours(int right_neibour_image_number, int left_neibour_image_number)
        {
            ObjectTypes right_type = project.Processes[current_process].images_in_tabItem[right_neibour_image_number].object_of_block.Type;
            ObjectTypes left_type = project.Processes[current_process].images_in_tabItem[left_neibour_image_number].object_of_block.Type;
            
            if (right_type == ObjectTypes.PROCEDURE)
                ProcedureToEverything(right_neibour_image_number, left_neibour_image_number);
            else if (left_type == ObjectTypes.PROCEDURE)
                ProcedureToEverything(left_neibour_image_number, right_neibour_image_number);
            else if (right_type == ObjectTypes.SUBPROCESS)
                SubProcessToEverything(right_neibour_image_number, left_neibour_image_number);
            else if (left_type == ObjectTypes.SUBPROCESS)
                SubProcessToEverything(left_neibour_image_number, right_neibour_image_number);
            else if (right_type == ObjectTypes.RESOURCE && left_type == ObjectTypes.PARALLEL_PROCESS)
            {
                //ресурс - процедура из параллельного процесса
            }
            else if (left_type == ObjectTypes.RESOURCE && right_type == ObjectTypes.PARALLEL_PROCESS)
            {
                //процедура из параллельного процесса - ресурс
            }
            else if (right_type == ObjectTypes.PARALLEL_PROCESS && left_type == ObjectTypes.SUBPROCESS)
            {
                //паралл процесс - вложенный процесс
            }
            else if (left_type == ObjectTypes.PARALLEL_PROCESS && right_type == ObjectTypes.SUBPROCESS)
            {
                //вложенный процесс - паралл процесс
            }
        }

//создаем соседей у процедур
        private void ProcedureToEverything(int right_neibour_image_number, int left_neibour_image_number)  //записать соседей
        {
            ObjectTypes right_type = project.Processes[current_process].images_in_tabItem[right_neibour_image_number].object_of_block.Type;
            ObjectTypes left_type = project.Processes[current_process].images_in_tabItem[left_neibour_image_number].object_of_block.Type;
            int right_neibour_type_number = project.Processes[current_process].images_in_tabItem[right_neibour_image_number].object_of_block.number;
            int left_neibour_type_number = project.Processes[current_process].images_in_tabItem[left_neibour_image_number].object_of_block.number;

            if (right_type == ObjectTypes.PROCEDURE) //если первый сосед - процедура
            {
                Neibour new_neibour;
                switch (left_type)
                {
                    case ObjectTypes.PROCEDURE:  //процедура - процедура
                        {
                            //создаем правого соседа-процедуру
                            new_neibour = new Neibour(right_type, right_neibour_type_number);
                            //Пишем туда, где NO_OBJECT, чтобы не перезаписывалось
                            if (project.Processes[current_process].Procedures[left_neibour_type_number].Right_Neibour.type == 
                                ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].Procedures[left_neibour_type_number].Right_Neibour = new_neibour;
                            else
                                project.Processes[current_process].Procedures[left_neibour_type_number].Left_Neibour = new_neibour;
                            //создаем левого соседа-процедуру
                            new_neibour = new Neibour(left_type, left_neibour_type_number);

                            if (project.Processes[current_process].Procedures[right_neibour_type_number].Left_Neibour.type ==
                                ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].Procedures[right_neibour_type_number].Left_Neibour = new_neibour;
                            else
                                project.Processes[current_process].Procedures[right_neibour_type_number].Right_Neibour = new_neibour;

                            break;
                        }
                    case ObjectTypes.RESOURCE:  //процедура - ресурс (добавляем номер ресурса в список ресурсов у процедуры)
                        {
                             project.Processes[current_process].Procedures[right_neibour_type_number].Resources.Add(left_neibour_type_number);
                             break;
                        }
                    case ObjectTypes.BEGIN:     //процедура - начало
                        {
                            new_neibour = new Neibour(left_type, left_neibour_type_number);
                            if (project.Processes[current_process].Procedures[right_neibour_type_number].Right_Neibour.type ==
                               ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].Procedures[right_neibour_type_number].Right_Neibour = new_neibour;
                            else
                                project.Processes[current_process].Procedures[right_neibour_type_number].Left_Neibour = new_neibour;
                            draw.SetLight(true);
                            break;
                        }                        
                    case ObjectTypes.END:      //процедура - конец
                        {
                            new_neibour = new Neibour(left_type, left_neibour_type_number);
                            if (project.Processes[current_process].Procedures[right_neibour_type_number].Right_Neibour.type ==
                              ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].Procedures[right_neibour_type_number].Right_Neibour = new_neibour;
                            else
                                project.Processes[current_process].Procedures[right_neibour_type_number].Left_Neibour = new_neibour;
                            break;
                        }                        
                    case ObjectTypes.SUBPROCESS:  //процедура - вложенный процесс
                        {
                            //создаем правого соседа-процедуру
                            new_neibour = new Neibour(right_type, right_neibour_type_number);
                            //Пишем туда, где NO_OBJECT, чтобы не перезаписывалось
                            if (project.Processes[current_process].SubProcesses[left_neibour_type_number].Right_Neibour.type ==
                                ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].SubProcesses[left_neibour_type_number].Right_Neibour = new_neibour;
                            else
                                project.Processes[current_process].SubProcesses[left_neibour_type_number].Left_Neibour = new_neibour;
                            //создаем левого соседа-процедуру
                            new_neibour = new Neibour(left_type, left_neibour_type_number);
                            if (project.Processes[current_process].Procedures[right_neibour_type_number].Left_Neibour.type ==
                                ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].Procedures[right_neibour_type_number].Left_Neibour = new_neibour;
                            else
                                project.Processes[current_process].Procedures[right_neibour_type_number].Right_Neibour = new_neibour;
                        }
                        break;
                    case ObjectTypes.PARALLEL_PROCESS:  //процедура - параллельный процесс
                        {
                        }
                        break;
                }                
            }            
        }

        public void SubProcessToEverything(int right_neibour_image_number, int left_neibour_image_number)
        {
            ObjectTypes right_type = project.Processes[current_process].images_in_tabItem[right_neibour_image_number].object_of_block.Type;
            ObjectTypes left_type = project.Processes[current_process].images_in_tabItem[left_neibour_image_number].object_of_block.Type;
            int right_neibour_type_number = project.Processes[current_process].images_in_tabItem[right_neibour_image_number].object_of_block.number;
            int left_neibour_type_number = project.Processes[current_process].images_in_tabItem[left_neibour_image_number].object_of_block.number;

            if (right_type == ObjectTypes.SUBPROCESS) //если первый сосед - подпроцесс
            {
                Neibour new_neibour;
                switch (left_type)
                {
                    case ObjectTypes.PROCEDURE:  //подпроцесс - процедура
                        {
                            //создаем правого соседа-процедуру
                            new_neibour = new Neibour(right_type, right_neibour_type_number);
                            //Пишем туда, где NO_OBJECT, чтобы не перезаписывалось
                            if (project.Processes[current_process].SubProcesses[left_neibour_type_number].Right_Neibour.type ==
                                ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].SubProcesses[left_neibour_type_number].Right_Neibour = new_neibour;
                            else
                                project.Processes[current_process].SubProcesses[left_neibour_type_number].Left_Neibour = new_neibour;
                            //создаем левого соседа-процедуру
                            new_neibour = new Neibour(left_type, left_neibour_type_number);
                            if (project.Processes[current_process].Procedures[right_neibour_type_number].Left_Neibour.type ==
                                ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].Procedures[right_neibour_type_number].Left_Neibour = new_neibour;
                            else
                                project.Processes[current_process].Procedures[right_neibour_type_number].Right_Neibour = new_neibour;
                            break;
                        }
                    case ObjectTypes.BEGIN:     //подпроцесс - начало
                        {
                            new_neibour = new Neibour(left_type, left_neibour_type_number);
                            if (project.Processes[current_process].SubProcesses[right_neibour_type_number].Right_Neibour.type ==
                               ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].SubProcesses[right_neibour_type_number].Right_Neibour = new_neibour;
                            else
                                project.Processes[current_process].SubProcesses[right_neibour_type_number].Left_Neibour = new_neibour;
                            draw.SetLight(true);
                            break;
                        }
                    case ObjectTypes.END:      //подпроцесс - конец
                        {
                            new_neibour = new Neibour(left_type, left_neibour_type_number);
                            if (project.Processes[current_process].SubProcesses[right_neibour_type_number].Right_Neibour.type ==
                              ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].SubProcesses[right_neibour_type_number].Right_Neibour = new_neibour;
                            else
                                project.Processes[current_process].SubProcesses[right_neibour_type_number].Left_Neibour = new_neibour;
                            break;
                        }
                    case ObjectTypes.SUBPROCESS:  //подпроцесс - вложенный процесс
                        {
                            //создаем правого соседа-процедуру
                            new_neibour = new Neibour(right_type, right_neibour_type_number);
                            //Пишем туда, где NO_OBJECT, чтобы не перезаписывалось
                            if (project.Processes[current_process].SubProcesses[left_neibour_type_number].Right_Neibour.type ==
                                ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].SubProcesses[left_neibour_type_number].Right_Neibour = new_neibour;
                            else
                                project.Processes[current_process].SubProcesses[left_neibour_type_number].Left_Neibour = new_neibour;
                            //создаем левого соседа-процедуру
                            new_neibour = new Neibour(left_type, left_neibour_type_number);
                            if (project.Processes[current_process].SubProcesses[right_neibour_type_number].Left_Neibour.type ==
                                ObjectTypes.NO_OBJECT)
                                project.Processes[current_process].SubProcesses[right_neibour_type_number].Left_Neibour = new_neibour;
                            else
                                project.Processes[current_process].SubProcesses[right_neibour_type_number].Right_Neibour = new_neibour;
                        }
                        break;
                    case ObjectTypes.PARALLEL_PROCESS:  //подпроцесс - параллельный процесс
                        {
                        }
                        break;
                }
            }            
        }

//изменение имени блока
        public void SetStructName(int number_in_images, string new_name)  
        {
            BlockObject bl = new BlockObject();

            bl = project.Processes[current_process].images_in_tabItem[number_in_images];
            bl.label.Text = new_name;
            bl.label.Width = bl.image.Width - 3;

            project.Processes[current_process].images_in_tabItem[number_in_images] = bl;  //изменение label на блоке

            //изменение имени блока в структуре
            int number_in_type = project.Processes[current_process].images_in_tabItem[number_in_images].object_of_block.number;
            switch (project.Processes[current_process].images_in_tabItem[number_in_images].object_of_block.Type)
            {
                case ObjectTypes.PROCEDURE:
                        project.Processes[current_process].Procedures[number_in_type].Name = new_name;
                        break;
            }
        }

//изменение типа процедуры
        public void SetTypeProcedure(int number_in_images, bool is_common, string common_type)
        {
            //изменение имени блока в структуре
            int number_in_type = project.Processes[current_process].images_in_tabItem[number_in_images].object_of_block.number;
            project.Processes[current_process].Procedures[number_in_type].is_common = is_common;
            project.Processes[current_process].Procedures[number_in_type].common_type = common_type;
        }

//изменение координат блока
        public void SetStructPoint(int number_in_images, Point new_point)  
        {
            BlockObject bl = new BlockObject();

            bl.label = project.Processes[current_process].images_in_tabItem[number_in_images].label;
            if (project.Processes[current_process].images_in_tabItem[number_in_images].object_of_block.Type == ObjectTypes.SUBPROCESS)
            {
                Canvas.SetLeft(project.Processes[current_process].images_in_tabItem[number_in_images].label, new_point.X - project.Processes[current_process].images_in_tabItem[number_in_images].image.Width / 2 + 15); //устанавливаем размеры канвы 
                Canvas.SetTop(project.Processes[current_process].images_in_tabItem[number_in_images].label, new_point.Y - project.Processes[current_process].images_in_tabItem[number_in_images].image.Height / 4 + 10);
            }
            else
            {
                Canvas.SetLeft(project.Processes[current_process].images_in_tabItem[number_in_images].label, new_point.X - project.Processes[current_process].images_in_tabItem[number_in_images].image.Width / 2 + 5); //устанавливаем размеры канвы 
                Canvas.SetTop(project.Processes[current_process].images_in_tabItem[number_in_images].label, new_point.Y - project.Processes[current_process].images_in_tabItem[number_in_images].image.Height / 4);
            }
            bl.object_of_block = project.Processes[current_process].images_in_tabItem[number_in_images].object_of_block;
            bl.object_of_block.point = new_point;
            bl.image = project.Processes[current_process].images_in_tabItem[number_in_images].image;
            Canvas.SetLeft(project.Processes[current_process].images_in_tabItem[number_in_images].image, new_point.X - project.Processes[current_process].images_in_tabItem[number_in_images].image.Width / 2); //устанавливаем размеры канвы 
            Canvas.SetTop(project.Processes[current_process].images_in_tabItem[number_in_images].image, new_point.Y - project.Processes[current_process].images_in_tabItem[number_in_images].image.Height / 2);

            project.Processes[current_process].images_in_tabItem[number_in_images] = bl;
        }

//изменение номера блока в соответствующем списке объектов типа
        public void SetStructNumber(int number_in_images, int new_number)  
        {
            BlockObject bl = new BlockObject();

            bl.label = project.Processes[current_process].images_in_tabItem[number_in_images].label;
            bl.object_of_block = project.Processes[current_process].images_in_tabItem[number_in_images].object_of_block;
            bl.object_of_block.number = new_number;
            bl.image = project.Processes[current_process].images_in_tabItem[number_in_images].image;

            project.Processes[current_process].images_in_tabItem[number_in_images] = bl;
        }

        public void SetStructTime(int number_in_images, double new_time)
        {
            project.Processes[current_process].Procedures[project.Processes[current_process].images_in_tabItem[number_in_images].object_of_block.number].Time_in_days = new_time;
        }

//удаление соседей 
        public void DeleteNeibours(int number_to_delete)
        {
            int number = project.Processes[current_process].images_in_tabItem[number_to_delete].object_of_block.number;            
            Neibour no_neibor = new Neibour(ObjectTypes.NO_OBJECT, -1);

            switch (project.Processes[current_process].images_in_tabItem[number_to_delete].object_of_block.Type)
            {
                case ObjectTypes.PROCEDURE: //то, что хотят удалить - процедура
                    {
                        //смотрим связанные с ней процедуры и удаляем у них соседа(удаляемую процедуру)
                        for (int i = 0; i < project.Processes[current_process].Procedures.Count; i++)
                            if (project.Processes[current_process].Procedures[i].Left_Neibour.number == number)
                                project.Processes[current_process].Procedures[i].Left_Neibour = no_neibor;
                            else if (project.Processes[current_process].Procedures[i].Right_Neibour.number == number)
                                project.Processes[current_process].Procedures[i].Right_Neibour = no_neibor;               
                    }
                    break;
                case ObjectTypes.SUBPROCESS:  //то, что хотят удалить - вложенный процесс
                    {
                        for (int i = 0; i < project.Processes[current_process].SubProcesses.Count; i++)
                            if (project.Processes[current_process].SubProcesses[i].Left_Neibour.number == number)
                                project.Processes[current_process].SubProcesses[i].Left_Neibour = no_neibor;
                            else if (project.Processes[current_process].SubProcesses[i].Right_Neibour.number == number)
                                project.Processes[current_process].SubProcesses[i].Right_Neibour = no_neibor;   
                    }
                    break;
                case ObjectTypes.PARALLEL_PROCESS: //то, что хотят удалить - параллельный процесс
                    {
                    }
                    break;
            }            
        }

//удаляем линии
        public void DeleteLine(ref Canvas canva, int number_to_delete)
        {
            for (int i = 0; i < project.Processes[current_process].connection_lines.Count; i++)
                if (project.Processes[current_process].connection_lines[i].block1 == number_to_delete || project.Processes[current_process].connection_lines[i].block2 == number_to_delete)
                {
                    if (project.Processes[current_process].connection_lines[i].block1 == 0 || project.Processes[current_process].connection_lines[i].block2 == 0)//гаснит лампочка при удалении связи
                        //которая с ней связана
                        draw.SetLight(false);//гасим лампочку
                    canva.Children.Remove(project.Processes[current_process].connection_lines[i].object_line);
                    project.Processes[current_process].connection_lines.RemoveAt(i);                    
                    i = -1;
                }
        }

//удаление ресурса у процедур
        public void DeleteResourseFromProcedure(int number_to_delete)
        {
            for (int i = 0; i < project.Processes[current_process].Procedures.Count; i++)//идем по процедурам
                for (int j = 0; j < project.Processes[current_process].Procedures[i].Resources.Count; j++)//идем по
                    if (project.Processes[current_process].Procedures[i].Resources[j] ==  //ресурсам текущей процедуры
                        project.Processes[current_process].images_in_tabItem[number_to_delete].object_of_block.number)
                        project.Processes[current_process].Procedures[i].Resources.RemoveAt(j);
        }

//удаление соседей при удалении связей
        public void DeleteNeibours_when_DeleteConnect(int current_connect)
        {
            int bk1 = project.Processes[current_process].connection_lines[current_connect].block1;
            int bk2 = project.Processes[current_process].connection_lines[current_connect].block2;
            int number_in_type1 = project.Processes[current_process].images_in_tabItem[bk1].object_of_block.number;
            int number_in_type2 = project.Processes[current_process].images_in_tabItem[bk2].object_of_block.number;

            Neibour no_neibor = new Neibour(ObjectTypes.NO_OBJECT, -1);

            switch (project.Processes[current_process].images_in_tabItem[bk1].object_of_block.Type)
            {
                case ObjectTypes.PROCEDURE:
                    switch (project.Processes[current_process].images_in_tabItem[bk2].object_of_block.Type)
                    {
                        case ObjectTypes.PROCEDURE:
                            {
                                //убираем соседа у первого блока
                                if (project.Processes[current_process].Procedures[number_in_type1].Left_Neibour.number == bk2)
                                    project.Processes[current_process].Procedures[number_in_type1].Left_Neibour = no_neibor;
                                else if (project.Processes[current_process].Procedures[number_in_type1].Right_Neibour.number == bk2)
                                    project.Processes[current_process].Procedures[number_in_type1].Right_Neibour = no_neibor;
                                //убираем соседа у второго блока
                                if (project.Processes[current_process].Procedures[number_in_type2].Left_Neibour.number == bk1)
                                    project.Processes[current_process].Procedures[number_in_type2].Left_Neibour = no_neibor;
                                else if (project.Processes[current_process].Procedures[number_in_type2].Right_Neibour.number == bk1)
                                    project.Processes[current_process].Procedures[number_in_type2].Right_Neibour = no_neibor;
                            }
                            break;
                        case ObjectTypes.RESOURCE:
                                project.Processes[current_process].Procedures[number_in_type1].Resources.RemoveAt(number_in_type2);
                            break;
                        case ObjectTypes.BEGIN:
                            {
                                if (project.Processes[current_process].Procedures[number_in_type1].Left_Neibour.number == 0)
                                    project.Processes[current_process].Procedures[number_in_type1].Left_Neibour = no_neibor;
                                else if (project.Processes[current_process].Procedures[number_in_type1].Right_Neibour.number == 0)
                                    project.Processes[current_process].Procedures[number_in_type1].Right_Neibour = no_neibor;
                                draw.SetLight(false);//гасим лампочку
                            }
                            break;
                        case ObjectTypes.END:
                            {
                                if (project.Processes[current_process].Procedures[number_in_type1].Left_Neibour.number == 1)
                                    project.Processes[current_process].Procedures[number_in_type1].Left_Neibour = no_neibor;
                                else if (project.Processes[current_process].Procedures[number_in_type1].Right_Neibour.number == 1)
                                    project.Processes[current_process].Procedures[number_in_type1].Right_Neibour = no_neibor;
                            }
                            break;
                        case ObjectTypes.SUBPROCESS:
                            {
                                //убираем соседа у первого блока
                                if (project.Processes[current_process].Procedures[number_in_type1].Left_Neibour.number == bk2)
                                    project.Processes[current_process].Procedures[number_in_type1].Left_Neibour = no_neibor;
                                else if (project.Processes[current_process].Procedures[number_in_type1].Right_Neibour.number == bk2)
                                    project.Processes[current_process].Procedures[number_in_type1].Right_Neibour = no_neibor;
                                //убираем соседа у второго блока
                                if (project.Processes[current_process].SubProcesses[number_in_type2].Left_Neibour.number == bk1)
                                    project.Processes[current_process].SubProcesses[number_in_type2].Left_Neibour = no_neibor;
                                else if (project.Processes[current_process].SubProcesses[number_in_type2].Right_Neibour.number == bk1)
                                    project.Processes[current_process].SubProcesses[number_in_type2].Right_Neibour = no_neibor;
                            }
                            break;
                        //если параллельный процесс
                    }
                    break;
                case ObjectTypes.RESOURCE:
                    project.Processes[current_process].Procedures[number_in_type2].Resources.RemoveAt(number_in_type1);
                    break;
                case ObjectTypes.BEGIN:
                    {
                        if (project.Processes[current_process].Procedures[number_in_type2].Left_Neibour.number == 0)
                            project.Processes[current_process].Procedures[number_in_type2].Left_Neibour = no_neibor;
                        else if (project.Processes[current_process].Procedures[number_in_type2].Right_Neibour.number == 0)
                            project.Processes[current_process].Procedures[number_in_type2].Right_Neibour = no_neibor;
                    }
                    break;
                case ObjectTypes.END:
                    {
                        if (project.Processes[current_process].Procedures[number_in_type2].Left_Neibour.number == 1)
                            project.Processes[current_process].Procedures[number_in_type2].Left_Neibour = no_neibor;
                        else if (project.Processes[current_process].Procedures[number_in_type2].Right_Neibour.number == 1)
                            project.Processes[current_process].Procedures[number_in_type2].Right_Neibour = no_neibor;
                    }
                    break;
                case ObjectTypes.SUBPROCESS:
                    {
                        //убираем соседа у первого блока
                        if (project.Processes[current_process].SubProcesses[number_in_type1].Left_Neibour.number == bk2)
                            project.Processes[current_process].SubProcesses[number_in_type1].Left_Neibour = no_neibor;
                        else if (project.Processes[current_process].SubProcesses[number_in_type1].Right_Neibour.number == bk2)
                            project.Processes[current_process].SubProcesses[number_in_type1].Right_Neibour = no_neibor;
                        //убираем соседа у второго блока
                        if (project.Processes[current_process].Procedures[number_in_type2].Left_Neibour.number == bk1)
                            project.Processes[current_process].Procedures[number_in_type2].Left_Neibour = no_neibor;
                        else if (project.Processes[current_process].Procedures[number_in_type2].Right_Neibour.number == bk1)
                            project.Processes[current_process].Procedures[number_in_type2].Right_Neibour = no_neibor;
                    }
                    break;
            }
        }

//обновление номеров блока-начала и блока-конца у линий после удаления блока
        public void RefreshNumbersLines(int deleted_block)
        {
            for (int i = 0; i < project.Processes[current_process].connection_lines.Count; i++)
            {
                if (project.Processes[current_process].connection_lines[i].block1 > deleted_block)
                {
                    int block1 = project.Processes[current_process].connection_lines[i].block1 - 1;
                    Connection_Line refreshed_line = new Connection_Line(block1, project.Processes[current_process].connection_lines[i].block2, project.Processes[current_process].connection_lines[i].object_line);
                    project.Processes[current_process].connection_lines[i] = refreshed_line;
                }
                if (project.Processes[current_process].connection_lines[i].block2 > deleted_block)
                {
                    int block2 = project.Processes[current_process].connection_lines[i].block2 - 1;
                    Connection_Line refreshed_line = new Connection_Line(project.Processes[current_process].connection_lines[i].block1, block2, project.Processes[current_process].connection_lines[i].object_line);
                    project.Processes[current_process].connection_lines[i] = refreshed_line;
                }
            }   
        }

//обновление номеров блоков в списке images_in_tabItem после удаления блока
        public void RefreshNumbersBlocks(int deleted_block)
        {
            for (int i = deleted_block; i < project.Processes[current_process].images_in_tabItem.Count; i++)
                if (project.Processes[current_process].images_in_tabItem[i].object_of_block.number > project.Processes[current_process].images_in_tabItem[deleted_block].object_of_block.number)
                {
                    BlockObject refreshed_structure = new BlockObject();
                    refreshed_structure = project.Processes[current_process].images_in_tabItem[i];
                    refreshed_structure.object_of_block.number -= 1;
                    project.Processes[current_process].images_in_tabItem[i] = refreshed_structure;
                }
        }

//обновление номеров блоков в списке Resources после удаления ресурса
        public void RefreshNumbersResources(int deleted_resources)
        {
           for (int i = 0; i < project.Processes[current_process].Procedures.Count; i++)
             for (int j = 0; j < project.Processes[current_process].Procedures[i].Resources.Count; j++)
                 if (project.Processes[current_process].Procedures[i].Resources[j]>deleted_resources)
                    project.Processes[current_process].Procedures[i].Resources[j] -= 1;
        }
    }
}
