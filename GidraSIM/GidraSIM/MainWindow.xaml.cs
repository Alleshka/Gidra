using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Forms;
using GidraSIM.AdmSet;

using CommonData;  //пространство структур системы

namespace GidraSIM
{
    public partial class MainWindow : Window
    {
        public Project project;                         //главный проект
        public ObjectTypes CurrentObject;
        public Canvas canvas_process;        //текущая канва для рисования
        public DrawShema drawing;
        public ChangeStruct changeStruct;
        public BlockProperties showProperties;
        public Find find;                               //определение, на какой элемент нажали
        public CheckCorrectProcess check_correct_process;
        public WorksystemWithFiles FilesWorksystem;
        TabItem currentTab;                             //текущая вкладка процесса 
        Messages message;                                //сообщения

        double x_tab;                                   //координаты расположения tabControl 
        double y_tab;

        bool mouse_pushed;
        bool project_create;                //признак, что процесс создали

        int how_many_process;                    //общее число процессов
        int current_process;         //номер текузего процесса
        int current_block;    //номер блока, который сейчас выделен
        int previous_block;   //номер блока, который был выделен ранее
        int current_connect;  //номер линии, которая сейчас выделена
        int previous_connect; //номер линии, которая была выделена ранее
        int left_neibour_image_number = -1;
        int right_neibour_image_number = -1;
        

        public MainWindow()
        {
            InitializeComponent();
            how_many_process = 0;
            project_create = false;
            message = new Messages(ref LabelMessageSystem, ref LabelMessageError, ref TabControl_Error);
            drawing = new DrawShema();
            FilesWorksystem = new WorksystemWithFiles();// работа с файлами проекта
            x_tab = 78;
            y_tab = 56;

            Settings set = SettingsReader.Read();
            if (set == null) System.Windows.Forms.MessageBox.Show($"Файл настроек не найден");
           
            // Привязка команд            
            
            // Стандартные команды
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, Create_Project_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Project_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Project_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed));

            // Кастомные команды
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Arrow, Arrow_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Procedure, Procedure_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Resourse, Resourse_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Connect, Connect_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.SubProcess, SubProcess_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.StartCheck, StartCheck_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.StartModeling, StartModeling_Executed));
        }


        //перевод общих координат точки в координаты на таб итеме----------------------------------------------------------------------------------- 
        private Point Point_in_TabPoint(Point pt)
        {
            pt.X = pt.X - x_tab;
            pt.Y = pt.Y - y_tab - 25;
            return pt;
        }

        //изменение вкладки--------------------------------------------------------------------------------------------------------------------------
        private void TabControl_Process_SelectionChanged(object sender, SelectionChangedEventArgs e)//выбор вкладки 
        {
            currentTab = TabControl_Process.SelectedValue as TabItem; //какую выбрали вкладку 
            currentTab.IsSelected = true;                             //сделали ее активной 
            current_process = TabControl_Process.SelectedIndex;
            canvas_process = currentTab.Content as Canvas;            //берем канву вкладки
            changeStruct.ChangeNumberProcess(current_process);
            drawing.ChangeCanva(canvas_process, ref project.Processes[current_process].images_in_tabItem);
        }

        //действия на поле------------------------------------------------------------------------------------------------------------
        private void TabControl_Process_MouseDown(object sender, MouseButtonEventArgs e) //нажали на поле 
        {
            if (!project_create)   ///если проект еще не создан, но ничего не делаем
            {
                message.ShowError(17);
                return;
            }

            Point pt = e.GetPosition(this);
            pt = Point_in_TabPoint(pt);//пересчет координат из глобальных в локальные

            if (CurrentObject == ObjectTypes.CURSOR)   //если выбран курсор
            {
                int what_block = find.WhatBlock(project.Processes[current_process].images_in_tabItem, pt);   //проверка, на какой блок нажали
                int what_connect = find.WhatConnect(project.Processes[current_process].connection_lines, pt);//проверка, на какую линию нажали
                drawing.DropShadowBlock(previous_block);//снимаем тень с предыдущего блока
                drawing.DropShadowLine(ref project.Processes[current_process].connection_lines, previous_connect);//снимаем тень с предыдущей линии

                if (what_block != -1)//если выбрали блок
                {
                    current_connect = previous_connect = -1; //снимаем тени с линий
                    drawing.DropShadowLine(ref project.Processes[current_process].connection_lines, current_connect);//снимаем тень с текущей линии
                    mouse_pushed = true;
                    previous_block = current_block;
                    current_block = what_block;
                    drawing.SetShadowBlock(current_block, previous_block);//подсвечиваем выбранный блок
                    //вызов свойств
                    grid_Properties.Children.Clear(); //очищаем свойства предыдущего блока
                    grid_Properties.RowDefinitions.Clear();
                    grid_Properties.ColumnDefinitions.Clear();
                    showProperties.ChooseProperties(current_process, project.Processes[current_process].images_in_tabItem[what_block].object_of_block.number,
                                                    project.Processes[current_process].images_in_tabItem[what_block].object_of_block.Type);

                    // drawing.SetShadowLine(ref connection_lines, 2, 1);//подсвечиваем выбранную связь
                }
                else if (what_connect != -1) //если выбрали линию
                {
                    current_block = previous_block = -1; //снимаем тени с блоков
                    drawing.DropShadowBlock(current_block);//снимаем тень с текущего блока
                    previous_connect = current_connect;
                    current_connect = what_connect;
                    drawing.SetShadowLine(ref project.Processes[current_process].connection_lines, current_connect, previous_connect);//подсвечиваем выбранную связь
                }
                else // нажали на пустое поле
                {
                    grid_Properties.Children.Clear();//очищаем блок свойств
                    drawing.DropShadowBlock(current_block);//снимаем тень с текущего блока
                    drawing.DropShadowLine(ref project.Processes[current_process].connection_lines, current_connect);//снимаем тень с текущей линии
                    
                    // снять фокус с текущего блока
                    current_block = -1;
                    // снять фокус с текущей линии
                    current_connect = -1;
                }
            }
            else if (CurrentObject == ObjectTypes.CONNECT)   //создание связи
                CreateConnection(pt);
            else                                            //создание блока
                if (CheckAddBlock(pt))
                    CreateBlock(pt);
                else                                        //вывод ошибки о том, что пользователь
                    message.ShowError(9);       //пытается поставить блоки слишком близко

        }

        //дважды щелкнули--------------------------------------------------------------------------------------------------------
        private void TabControl_Process_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }

            Point pt = e.GetPosition(this);
            pt = Point_in_TabPoint(pt);//пересчет координат из глобальных в локальные

            if (CurrentObject == ObjectTypes.CURSOR)   //если выбран курсор
            {
                int what_block = find.WhatBlock(project.Processes[current_process].images_in_tabItem, pt);   //проверка, на какой блок нажали
                int what_connect = find.WhatConnect(project.Processes[current_process].connection_lines, pt);//проверка, на какую линию нажали
                drawing.DropShadowBlock(previous_block);//снимаем тень с предыдущего блока
                drawing.DropShadowLine(ref project.Processes[current_process].connection_lines, previous_connect);//снимаем тень с предыдущей линии

                if (what_block != -1) //если выбрали блок
                {
                    current_connect = previous_connect = -1; //снимаем тени с линий
                    drawing.DropShadowLine(ref project.Processes[current_process].connection_lines, current_connect);//снимаем тень с текущей линии
                    previous_block = current_block;
                    current_block = what_block;
                    drawing.SetShadowBlock(current_block, previous_block); //подсвечиваем выбранный блок

                    if (project.Processes[current_process].images_in_tabItem[what_block].object_of_block.Type == ObjectTypes.RESOURCE)//если ресурс
                    {
                        SetResource window = new SetResource();  //вызываем окно выбора ресурса
                        window.ShowDialog();
                        ResourceTypes resource_type = window.selectedType;
                        int resource_id = window.selectedId;
                        //назначаем ресурс в списке ресурсов, если он был выбран
                        if (resource_id != -1)
                        {
                            project.Processes[current_process].Resources[project.Processes[current_process].images_in_tabItem[current_block].object_of_block.number].Type = resource_type;
                            project.Processes[current_process].Resources[project.Processes[current_process].images_in_tabItem[current_block].object_of_block.number].id = resource_id;
                            drawing.SetResourceImage(resource_type, resource_id, current_block);
                        }
                    }
                    if (project.Processes[current_process].images_in_tabItem[what_block].object_of_block.Type == ObjectTypes.SUBPROCESS)//если подпроцесс
                    {
                        SetSubProcess window = new SetSubProcess(project, current_process);
                        project.Processes[current_process].SubProcesses[project.Processes[current_process].images_in_tabItem[what_block].object_of_block.number].number_in_processes = window.chosen_process_number;
                        project.Processes[current_process].images_in_tabItem[what_block].label.Text = project.Processes[window.chosen_process_number].Name;
                    }
                    else if (project.Processes[current_process].images_in_tabItem[what_block].object_of_block.Type == ObjectTypes.PROCEDURE)//если процедура
                    {
                        SetProcedure window = new SetProcedure(ref project, project.Processes[current_process].images_in_tabItem[what_block].object_of_block.number, current_process);
                        window.ShowDialog();
                        changeStruct.SetStructName(what_block,
                            project.Processes[current_process].Procedures[project.Processes[current_process].images_in_tabItem[current_block].object_of_block.number].Name);
                    }
                }
            }
        }

        //отпустили кнопку мыши-----------------------------------------------------------------------------------------------------
        private void TabControl_Process_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouse_pushed = false;
        }

        //мышка движется-------------------------------------------------------------------------------------------------------------
        private void TabControl_Process_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mouse_pushed)  //если кнопка мыши нажата - тянем блок и его связи(если есть)
            {
                Point pt = e.GetPosition(this);//координаты положения мыши
                pt = Point_in_TabPoint(pt);//пересчет координат из глобальных в локальные

                changeStruct.SetStructPoint(current_block, pt);
                MoveLine(pt);//едет линия за блоком
            }
        }

        //едет линия------------------------------------------------------------------------------------------------------------------
        private void MoveLine(Point move)
        {
            List<int[]> move_lines = new List<int[]>();      //1 элемент - номер линии, 2 - начало или конец
            for (int i = 0; i < project.Processes[current_process].connection_lines.Count; i++)  //ищем номера линий, связанных с движущимся блоком
            {
                if (project.Processes[current_process].connection_lines[i].block1 == current_block)  //текущий блок - это начало линии
                {
                    int[] massiv = new int[2];
                    massiv[0] = i;
                    massiv[1] = 1;          //1 - начало (начало линии связано с блоком)
                    move_lines.Add(massiv);
                }
                if (project.Processes[current_process].connection_lines[i].block2 == current_block)   //текущий блок - это конец линии
                {
                    int[] massiv = new int[2];
                    massiv[0] = i;
                    massiv[1] = 2;           //2 - конец (конец линии связан с блоком)
                    move_lines.Add(massiv);
                }
            }

            for (int i = 0; i < move_lines.Count; i++)
            {
                if (move_lines[i][1] == 1)   //если левая связь есть
                {
                    project.Processes[current_process].connection_lines[move_lines[i][0]].object_line.X1 = move.X;  //меняем коордиталы ее начала
                    project.Processes[current_process].connection_lines[move_lines[i][0]].object_line.Y1 = move.Y;
                }
                if (move_lines[i][1] == 2)   //если правая связь есть
                {
                    project.Processes[current_process].connection_lines[move_lines[i][0]].object_line.X2 = move.X; //меняем координаты ее конца
                    project.Processes[current_process].connection_lines[move_lines[i][0]].object_line.Y2 = move.Y;
                }
            }
        }

        //работа с блоками----------------------------------------------------------------------------------------------------------------
        //проверка, можно ли сюда поставить блок
        private bool CheckAddBlock(Point pt)
        {
            double rast = (int)Size_Block.WIDTH_BLOCK;
            //если пользователь пытается поставить блок на уже существующий, то нельзя
            for (int i = 0; i < project.Processes[current_process].images_in_tabItem.Count; i++)
            {// смотрим расстояние между новой точкой и серединой блока по ОХ и ОУ
                //считаем расстояние между точкой и блоком
                double D = Math.Sqrt(Math.Pow(pt.X - project.Processes[current_process].images_in_tabItem[i].object_of_block.point.X, 2) +
                    Math.Pow(pt.Y - project.Processes[current_process].images_in_tabItem[i].object_of_block.point.Y, 2));
                if (D < rast)
                {
                    message.ShowError(9);       //пытается поставить блоки слишком близко
                    return false;
                }
            }
            //если пользователь пытается поставить блок за границы поля
            if ((pt.X < (rast / 2)) || (pt.X > (TabControl_Process.ActualWidth - rast / 2)))
            {
                message.ShowError(10);       //пытается поставить блоки слишком близко к границе
                return false;
            }
            if ((pt.Y < (rast / 2)) || (pt.Y > (TabControl_Process.ActualHeight - rast / 2)))
            {
                message.ShowError(10);       //пытается поставить блоки слишком близко к границе
                return false;
            }
            return true;
        }

        //создание блока------------------------------------------------------------------------------------------------------------------
        private void CreateBlock(Point point)
        {
            try
            {
                if (TabControl_Process.Items.Count > 0) //если открыт один или больше процессов
                {
                    int number = 0;
                    string name = null;
                    if (CurrentObject == ObjectTypes.PROCEDURE)
                    {
                        Procedure procedure = new Procedure();
                        procedure.Name = "Процедура" + Convert.ToString(project.Processes[current_process].Procedures.Count + 1);
                        project.Processes[current_process].Procedures.Add(procedure);
                        number = project.Processes[current_process].Procedures.Count - 1;
                    }
                    else if (CurrentObject == ObjectTypes.RESOURCE)
                    {
                        Resource resource = new Resource();
                        project.Processes[current_process].Resources.Add(resource);
                        number = project.Processes[current_process].Resources.Count - 1;
                    }
                    else if (CurrentObject == ObjectTypes.SUBPROCESS)
                    {
                        SubProcess subProcess = new SubProcess();
                        SetSubProcess window = new SetSubProcess(project, current_process);
                        window.ShowDialog();
                        subProcess.number_in_processes = window.chosen_process_number;
                        if (subProcess.number_in_processes == -1) throw new Exception("Не выбрано подпроцесса");
                        project.Processes[current_process].SubProcesses.Add(subProcess);
                        name = project.Processes[window.chosen_process_number].Name;
                        project.Processes[window.chosen_process_number].IsSub = current_process;
                        number = project.Processes[current_process].SubProcesses.Count - 1;
                    }

                    drawing.AddBlock(CurrentObject, point, number, name); //добавляем блоки
                    canvas_process.Children.Add(project.Processes[current_process].images_in_tabItem[project.Processes[current_process].images_in_tabItem.Count - 1].image);
                    canvas_process.Children.Add(project.Processes[current_process].images_in_tabItem[project.Processes[current_process].images_in_tabItem.Count - 1].label);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        //проверка, можно ли соединить блоки------------------------------------------------------------------------------------------------------
        private bool CheckConnection()
        {
            ObjectTypes right_type = project.Processes[current_process].images_in_tabItem[right_neibour_image_number].object_of_block.Type;
            ObjectTypes left_type = project.Processes[current_process].images_in_tabItem[left_neibour_image_number].object_of_block.Type;

            //если хотят соединить начало с ресурсом - нельзя(false)
            if (right_type == ObjectTypes.BEGIN && left_type == ObjectTypes.RESOURCE ||
                left_type == ObjectTypes.BEGIN && right_type == ObjectTypes.RESOURCE)
            {
                message.ShowError(13);
                return false;
            }
            //начало с концом
            else if (right_type == ObjectTypes.BEGIN && left_type == ObjectTypes.END ||
                left_type == ObjectTypes.BEGIN && right_type == ObjectTypes.END)
            {
                message.ShowError(11);
                return false;
            }
            //конец с ресурсом
            else if (right_type == ObjectTypes.END && left_type == ObjectTypes.RESOURCE ||
                left_type == ObjectTypes.END && right_type == ObjectTypes.RESOURCE)
            {
                message.ShowError(14);
                return false;
            }
            //если хотят соединить ресурс с ресурсом
            else if (right_type == ObjectTypes.RESOURCE && right_type == left_type)
            {
                message.ShowError(12);
                return false;
            }
            else
                return true;
        }

        //соединение блоков-------------------------------------------------------------------------------------------------------
        private void CreateConnection(Point point)
        {
            int what_block = find.WhatBlock(project.Processes[current_process].images_in_tabItem, point);   //проверка, на какой блок нажали   
            if (what_block != -1)  //на кого нажали первым
            {
                drawing.DropShadowBlock(previous_block);
                if (right_neibour_image_number == -1)                              //если это первый, на который нажали, запоминаем его номер
                {
                    //снимаем тень
                    drawing.SetShadowBlock(what_block, previous_block);
                    previous_block = what_block;

                    right_neibour_image_number = what_block;
                }
                else  //если это второй, на который нажали, создаем связь
                {
                    //снимаем тень
                    drawing.SetShadowBlock(what_block, previous_block);
                    previous_block = what_block;

                    left_neibour_image_number = what_block;

                    if (right_neibour_image_number != left_neibour_image_number && CheckConnection())
                    {
                        changeStruct.SetStructNeibours(right_neibour_image_number, left_neibour_image_number);

                        //точки начала и конца линии
                        Point p_1 = project.Processes[current_process].images_in_tabItem[left_neibour_image_number].object_of_block.point;
                        Point p_2 = project.Processes[current_process].images_in_tabItem[right_neibour_image_number].object_of_block.point;
                        //создание объекта Линия(сама линия и номера блоков начала и конца)
                        Connection_Line connection = new Connection_Line(left_neibour_image_number, right_neibour_image_number,
                                                                         drawing.DrawLine(p_1, p_2));
                        project.Processes[current_process].connection_lines.Add(connection);
                        canvas_process.Children.Add(project.Processes[current_process].connection_lines[project.Processes[current_process].connection_lines.Count - 1].object_line);
                    }
                    left_neibour_image_number = right_neibour_image_number = -1;
                }
            }
        }

        //удаление объектов----------------------------------------------------------------------------------------------------------
        //удаление блока и его связей
        private void DeleteBlock()
        {
            switch (project.Processes[current_process].images_in_tabItem[current_block].object_of_block.Type)
            {
                case ObjectTypes.PROCEDURE:
                    {
                        changeStruct.DeleteLine(ref canvas_process, current_block); //удаляем связь
                        changeStruct.RefreshNumbersLines(current_block);//если был удален блок - 
                        //обновляем номера блоков начала и конца у линий
                        changeStruct.DeleteNeibours(current_block); //удаляем соседей  
                        project.Processes[current_process].Procedures.RemoveAt(project.Processes[current_process].images_in_tabItem[current_block].object_of_block.number);
                        
                        changeStruct.RefreshNumbersBlocks(current_block);
                        drawing.ClearBlock(current_block);
                        project.Processes[current_process].images_in_tabItem.RemoveAt(current_block);
                        current_block = previous_block = -1;
                    }
                    break;
                case ObjectTypes.RESOURCE:
                    {
                        changeStruct.DeleteLine(ref canvas_process, current_block); //удаляем связь
                        changeStruct.RefreshNumbersLines(current_block);//обновляем номера связей

                        changeStruct.DeleteResourseFromProcedure(current_block);//удаляем ресурс из процедур в которых он есть
                        changeStruct.RefreshNumbersResources(project.Processes[current_process].images_in_tabItem[current_block].object_of_block.number);//обновляем номера

                        project.Processes[current_process].Resources.RemoveAt(project.Processes[current_process].images_in_tabItem[current_block].object_of_block.number);

                        changeStruct.RefreshNumbersBlocks(current_block);
                        drawing.ClearBlock(current_block);
                        project.Processes[current_process].images_in_tabItem.RemoveAt(current_block);

                        current_block = previous_block = -1;//снимаем выделение с блоков и линий
                    }
                    break;
                case ObjectTypes.SUBPROCESS:
                    {
                        changeStruct.DeleteLine(ref canvas_process, current_block); //удаляем связь
                        changeStruct.RefreshNumbersLines(current_block);//обновляем номера связей

                        project.Processes[current_process].SubProcesses.RemoveAt(project.Processes[current_process].images_in_tabItem[current_block].object_of_block.number);

                        changeStruct.RefreshNumbersBlocks(current_block);
                        drawing.ClearBlock(current_block);
                        project.Processes[current_process].images_in_tabItem.RemoveAt(current_block);

                        current_block = previous_block = -1;//снимаем выделение с блоков и линий
                    }
                    break;
                case ObjectTypes.BEGIN:
                    message.ShowError(15);
                    break;
                case ObjectTypes.END:
                    message.ShowError(16);
                    break;

                //тут еще подпроцесс
                //и параллельные процессы
            }
        }

        //удаление только связи-------------------------------------------------------------------------------------------------------
        private void DeleteConnect()
        {
            changeStruct.DeleteNeibours_when_DeleteConnect(current_connect);
            canvas_process.Children.Remove(project.Processes[current_process].connection_lines[current_connect].object_line);
            project.Processes[current_process].connection_lines.RemoveAt(current_connect);
            current_connect = previous_connect = -1;
        }

        //кнопки на панели инструментов-------------------------------------------------------------------------------------------------------------
        private void Procedure_Executed(object sender, RoutedEventArgs e)  //добавление процедуры
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }
            CurrentObject = ObjectTypes.PROCEDURE;

            ChangeButtonMark(button_procedure);

            drawing.DropShadowBlock(current_block);
        }

        private void Resourse_Executed(object sender, RoutedEventArgs e)  //добавление ресурса
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }
            CurrentObject = ObjectTypes.RESOURCE;

            ChangeButtonMark(button_resourse);

            drawing.DropShadowBlock(current_block);
        }

        private void Connect_Executed(object sender, RoutedEventArgs e)   //добавление связи
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }
            CurrentObject = ObjectTypes.CONNECT;

            ChangeButtonMark(button_connect);

            drawing.DropShadowBlock(current_block);
        }



        private void Arrow_Executed(object sender, RoutedEventArgs e)    //выбор указателя
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }
            CurrentObject = ObjectTypes.CURSOR;

            ChangeButtonMark(button_arrow);

            drawing.DropShadowBlock(current_block);
        }

        private void SubProcess_Executed(object sender, RoutedEventArgs e)  //добавление подпроцесса
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }
            CurrentObject = ObjectTypes.SUBPROCESS;

            ChangeButtonMark(button_SubProcess);

            drawing.DropShadowBlock(current_block);
        }

        private void ChangeButtonMark(System.Windows.Controls.Button thusButton)
        {
            foreach (System.Windows.Controls.Button button in leftButtons.Children)
            {
                button.IsEnabled = true;
                (button.Content as UIElement).Opacity = 0.8;
            }
            thusButton.IsEnabled = false;
            (thusButton.Content as UIElement).Opacity = 0.2;
        }

        //---МЕНЮ--------------------------------------------------------------------------------------------------------------------------
        private void CreateProject_menu(object sender, RoutedEventArgs e)   //создать проект
        {
            CreateProject();
        }

        //создание нового процесса---------------------------------------------------------------------------------------------------
        private void CreateProcess_menu_Click(object sender, RoutedEventArgs e)
        {
            CreateProcess();
        }

        private void Redact_ResoursesDB_menu_Click(object sender, RoutedEventArgs e)
        {
            ChooseTable_ResoursesDB window = new ChooseTable_ResoursesDB();
            window.ShowDialog();
        }

        private void See_ModelSessionDB_menu_Click(object sender, RoutedEventArgs e)
        {
            SeeModelSessionDB window = new SeeModelSessionDB();
            window.ShowDialog();
        }

        private void Help_menu_Click(object sender, RoutedEventArgs e)//вызов справки
        {
            Help_Click(sender, e);
        }

        private void Properties_menu_Click(object sender, RoutedEventArgs e)//выбрали пункт меню Вид->Свойства
        {
            if (Properties_menu.IsChecked)
                TabControl_Properties.Visibility = System.Windows.Visibility.Visible;
            else
                TabControl_Properties.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Tree_menu_Click(object sender, RoutedEventArgs e)//выбрали пункт меню Вид->Структура
        {
            if (Tree_menu.IsChecked) 
                TabControl_Tree.Visibility = System.Windows.Visibility.Visible;
            else
                TabControl_Tree.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Error_menu_Click(object sender, RoutedEventArgs e)//выбрали пункт меню Вид->Ошибки
        {
            if (Error_menu.IsChecked)
                TabControl_Error.Visibility = System.Windows.Visibility.Visible;
            else
                TabControl_Error.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Toolbar_menu_Click(object sender, RoutedEventArgs e)//выбрали пункт меню Вид->Панель инструментов
        {
            if (Toolbar_menu.IsChecked)
                TabControl_Toolbar.Visibility = System.Windows.Visibility.Visible;
            else
                TabControl_Toolbar.Visibility = System.Windows.Visibility.Hidden;
        }


        //Запускаем проверку построения процесса
        private void StartCheck_Executed(object sender, RoutedEventArgs e)
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }
            //очищаем предыдущие ошибки перед новыми
            LabelMessageError.Content="";
            LabelMessageSystem.Content=null;

            check_correct_process = new CheckCorrectProcess(project, current_process, message);
            if (check_correct_process.CheckRoute())
            {
                if (check_correct_process.CheckProceduresOnResources())
                {
                    drawing.SetOk(true);
                    message.ShowMessage(1);
                    check_correct_process.CreateStruct(); //Запускаем построение структуры процесса
                }
                else
                {
                    drawing.SetOk(false);
                    message.ShowError(2);//щшибка
                }
            }
            else
            {
                if (!check_correct_process.CheckProceduresOnResources())
                {
                    message.ShowError(2);
                }
                drawing.SetOk(false);
                message.ShowError(1);
            }
        }
        

        private void StartModeling_Executed(object sender, RoutedEventArgs e)//Запускаем моделирование процесса
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }
            StartCheck_Executed(sender, e);//Запускаем проверку построения процесса
            Modeling window = new Modeling(ref project, current_process);
            if (window.stop)
                message.ShowError(19);
            else
                window.ShowDialog();
        }

        private void Delete_Executed(object sender, RoutedEventArgs e)//выбрали пункт меню Удалить
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }
            if (current_block != -1)
                DeleteBlock();
            else if (current_connect != -1)
                DeleteConnect();
        }


        //иконки-------------------------------------------------------------------------------------------------------------------------------
        private void Create_Project_Executed(object sender, RoutedEventArgs e)
        {
            CreateProject();
        }
        private void CreateProcess_Click(object sender, RoutedEventArgs e)
        {
            CreateProcess();
        }  
      

        //вызов справки------------------------------------------------------------------------------------------------------------------------
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            // drawing.DropShadowBlock(current_block); <--- Не понял, для чего это, но без этого программа не крашится при открытии справки 
            try
            {
                Process SysInfo = new Process();
                SysInfo.StartInfo.ErrorDialog = true;
                SysInfo.StartInfo.FileName = "HelpGidraSIM.chm"; //файл со справкой <--- Пихнём в установщик
                SysInfo.Start();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Не удалось открыть справку!");
            }
        }

        private void treeView_structure_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        //Если изменили размеры окна, меняем x_tab и y_tab----------------------------------------------------------------------------------------
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //x_tab = TabControl_Process.Margin.Left; //выясняем координаты левого верхнего угла tabControl 
            //y_tab = TabControl_Process.Margin.Top;
        }

        /// <summary>
        /// Поиск значения переменных x_tab и y_tab
        /// </summary>
        private void CalculateXY()
        {
            //this.x_tab = TabControl_Process.
            //this.canvas_process.
        }

        //сохранение проекта--------------------------------------------------------------------------------------------------------------------
        private void Save_Project_Executed(object sender, RoutedEventArgs e)
        {
            SaveProject();
        }


        //открытие проекта----------------------------------------------------------------------------------------------------------------
        private void Open_Project_Executed(object sender, RoutedEventArgs e)  //открыть проект
        {
            OpenProject();
        }

        //вводим параметры моделирования
        private void ParametersModeling_Menu_Click(object sender, RoutedEventArgs e)
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }
            ModelingParameters window = new ModelingParameters(ref project.modelingProperties);
            window.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           // DataBase_ModelingSession data_base = new DataBase_ModelingSession();
          //  data_base.ClearTables();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // -------------------------------------------- Это можно бы вынести отсюда -------------------------------------------------------------

        /// <summary>
        /// Открыть проект
        /// </summary>
        private void OpenProject()
        {
            if (project_create)   ///если проект уже создан
            {
                message.ShowError(7); //ошибка, проект уже создан, сначала закройте проект, а потом открывате 
                return;
            }

            message.ShowMessage(5); //сообщение о процессе загрузки проекта            
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "СИМ |*.gsim" + "|Все файлы (*.*)|*.* ";   // задаем допустимые расширения открываемых файлов
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = false;     //нельзя выбрать сразу несколько

            if (myDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string way = myDialog.FileName.Substring(0, myDialog.FileName.Length - myDialog.SafeFileName.Length - 1); // -1 для того, чтобы убрать "//"
                FilesWorksystem.UpdateWay(way, myDialog.SafeFileName);                //обновляем путь
                project = FilesWorksystem.OpenProject(myDialog.FileName);                     // открываем проект
                find = new Find();
                changeStruct = new ChangeStruct(ref project, ref drawing, current_process);
                showProperties = new BlockProperties(grid_Properties, ref project);
                RecreateProjectFromOpened renessans = new RecreateProjectFromOpened(ref project, ref TabControl_Process);//воссоздаем проект

                how_many_process = project.Processes.Count;
                currentTab = TabControl_Process.Items[0] as TabItem; //какую выбрали вкладку 
                currentTab.IsSelected = true;                             //сделали ее активной 
                current_process = TabControl_Process.SelectedIndex;
                canvas_process = currentTab.Content as Canvas;            //берем канву вкладки
                changeStruct.ChangeNumberProcess(current_process);
                drawing.ChangeCanva(canvas_process, ref project.Processes[current_process].images_in_tabItem);

                project_create = true;//проект открыт, а значит создан!
                message.ShowMessage(6);//сообщение о успешном открытии проекта
                CreateProcess_menu.IsEnabled = true;
            }
        }

        /// <summary>
        /// Создание проекта
        /// </summary>
        private void CreateProject()
        {
            CreateProject window = new CreateProject(ref FilesWorksystem);
            window.ShowDialog();

            project = new Project(window.NamePr, window.WayFile);
            CreateProcess_menu.IsEnabled = true;

            CreateProcess(); // Тут же вызываем окно создания процесса 
            SaveProject(); // Тут же сохраняем, чтобы появился файл проекта
            
        }

        /// <summary>
        /// Создание процесса
        /// </summary>
        private void CreateProcess()
        {
            if (project == null)//проект еще не создан
            {
                message.ShowError(18);
                return;
            }

            CreateProcess window = new CreateProcess(how_many_process + 1);   //вызов диалога нового процесса
            window.ShowDialog();
            Process_ process = new Process_(window.NamePr);
            project.Processes.Add(process);
            current_process = project.Processes.Count - 1;  //берем последний добаввелнный процесс
            find = new Find();
            changeStruct = new ChangeStruct(ref project, ref drawing, current_process);
            showProperties = new BlockProperties(grid_Properties, ref project);

            canvas_process = new Canvas(); //новая канва для рисования процесса 

            drawing.ChangeCanva(canvas_process, ref project.Processes[current_process].images_in_tabItem); //говорим, что будем рисовать тут             

            StructureObject begin = new StructureObject();  //создаем объект "Начало"
            begin.Type = ObjectTypes.BEGIN;
            begin.number = -2;
            begin.point = new Point(25, TabControl_Process.ActualHeight / 2);

            StructureObject end = new StructureObject();   //создаем объект "Конец"
            end.Type = ObjectTypes.END;
            end.number = -2;
            end.point = new Point(TabControl_Process.ActualWidth - 25, TabControl_Process.ActualHeight / 2);

            project.Processes[current_process].StructProcess.Add(begin);  //добавляем объект "Начало" в структуру процесса
            project.Processes[current_process].StructProcess.Add(end);    //добавляем объект "Конец" в структуру процесса

            currentTab = new TabItem();
            TabControl_Process.Items.Add(currentTab); //добавили новую вкладку 
            currentTab = TabControl_Process.Items[how_many_process] as TabItem; //берем только что добавленную вкладку 
            currentTab.Header = new TextBlock { Text = process.Name };

            drawing.AddBeginEnd(TabControl_Process.ActualWidth, TabControl_Process.ActualHeight); //создание начала и конца  

            for (int i = 0; i < project.Processes[current_process].images_in_tabItem.Count; i++)   //кладем начало и конец в канву
            {
                canvas_process.Children.Add(project.Processes[current_process].images_in_tabItem[i].image);
                canvas_process.Children.Add(project.Processes[current_process].images_in_tabItem[i].label);
            }
            currentTab.Content = canvas_process;
            currentTab.IsSelected = true; //сделали вкладку активной 
            how_many_process++;

            //дерево процессов
            TreeViewItem item = new TreeViewItem();
            item.Header = project.Processes[current_process].Name;
            treeView_structure.Items.Add(item);
            project_create = true;              //процесс создали, меняем флаг
        }

        /// <summary>
        /// Сохранение проекта
        /// </summary>
        private void SaveProject()
        {
            if (!project_create) //проект еще не создан
            {
                message.ShowError(17);
                return;
            }
            FilesWorksystem.SaveProject(ref project);
            message.ShowMessage(4);
        }

    }
}
