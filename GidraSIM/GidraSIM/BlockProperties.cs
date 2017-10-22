using System.Collections.Generic;
using CommonData;
using System.Windows;
using System.Windows.Controls;

namespace GidraSIM
{
    public class BlockProperties  //показ свойств блока
    {
        DataBase_Resourses dataBase;
        Grid grid_properties;
        Project project;
        int current_block;//номер блока, по которому показываем свойства
        int current_process;  //номер текущего процесса

        int fontSizeHeader = 14; //размер шрифта заголовка свойства
        int fontSizeText = 14;   //размер шрифта текста
        int text_height = 35;    //высота ячейки
        int text_wight;          //ширина ячейки 

        public BlockProperties(Grid grid, ref Project new_project)
        {
            grid_properties = new Grid();
            grid_properties = grid;
            project = new_project;
            dataBase = new DataBase_Resourses();

            text_wight = (int)grid_properties.ActualWidth / 2;
        }

        //какие свойства выводить (по типу блока)
        public void ChooseProperties(int number_process, int number_block, ObjectTypes type)
        {
            current_process = number_process;
            current_block = number_block;//запоминаем тип и номер текущего блока
            
            switch (type)
            {
                case ObjectTypes.PROCEDURE: ShowPropertiesProsedure();
                    break;
                case ObjectTypes.RESOURCE: ShowPropertiesResourse();
                    break;
            }
        }

        //делаем TextBlock
        private TextBlock CreateTextBlock(string text, int what)
        {
            TextBlock new_textBlock = new TextBlock();
            new_textBlock.Width = text_wight;
            new_textBlock.TextWrapping = TextWrapping.Wrap;
            new_textBlock.Text = text;
            if (what == 1)                      //если заголовок
            {
                new_textBlock.FontSize = fontSizeHeader;
                new_textBlock.FontWeight = FontWeights.Bold;
            }
            else
                new_textBlock.FontSize = fontSizeText;

            return new_textBlock;
        }

        //свойства процедуры
        public void ShowPropertiesProsedure()
        {
            Procedure procedure = new Procedure();
            procedure = project.Processes[current_process].Procedures[current_block]; //берем нужную процедуру   

            //создание таблицы
            GridLength gridHeight = new GridLength(text_height, GridUnitType.Pixel);  //установка высоты ячеек
            ColumnDefinition column1 = new ColumnDefinition(); //добавляем столбцы
            ColumnDefinition column2 = new ColumnDefinition();
            grid_properties.ColumnDefinitions.Add(column1); 
            grid_properties.ColumnDefinitions.Add(column2);

            RowDefinition row_name = new RowDefinition();  //добавляем строки
            RowDefinition row_type = new RowDefinition();
            RowDefinition row_resourses = new RowDefinition();
            grid_properties.RowDefinitions.Add(row_name);
            grid_properties.RowDefinitions.Add(row_type);
            row_resourses.Height = gridHeight;
            grid_properties.RowDefinitions.Add(row_resourses);         
            //заголовки таблицы            
            TextBlock header1 = CreateTextBlock("Имя", 1);  //имя
            Grid.SetColumn(header1, 0);
            Grid.SetRow(header1, 0);
            grid_properties.Children.Add(header1);
            
            TextBlock header2 = CreateTextBlock("Инфо", 1);//тип
            Grid.SetColumn(header2, 0);
            Grid.SetRow(header2, 1);
            grid_properties.Children.Add(header2);
            
            TextBlock header4 = CreateTextBlock("Ресурсы", 1);//ресурсы
            Grid.SetColumn(header4, 0);
            Grid.SetRow(header4, 3);
            grid_properties.Children.Add(header4);

            //////заполение таблицы///////////////            
            TextBlock text_name = CreateTextBlock(procedure.Name, 2);//имя
            Grid.SetColumn(text_name, 1);
            Grid.SetRow(text_name, 0);
            grid_properties.Children.Add(text_name);
            
            TextBlock text_type;//тип
            if (procedure.is_common) //если типовая - выводим тип, если нет - время выполнения
                text_type = CreateTextBlock(procedure.common_type, 2);
            else
                text_type = CreateTextBlock(procedure.Time_in_format, 2);
            Grid.SetColumn(text_type, 1);
            Grid.SetRow(text_type, 1);
            grid_properties.Children.Add(text_type);
            
            ComboBox resourses = new ComboBox();//ресурсы
            resourses.Width = text_wight;
            List<string> list = new List<string>();
            for (int i = 0; i < procedure.Resources.Count; i++)
            {
                int num = procedure.Resources[i];
                list.Add(dataBase.GetRowName(project.Processes[current_process].Resources[num].Type,
                    project.Processes[current_process].Resources[num].id));  //имя из табоицы по id
            }
            resourses.ItemsSource = list;
            Grid.SetColumn(resourses, 1);
            Grid.SetRow(resourses, 2);
            grid_properties.Children.Add(resourses);
        }

        //свойства ресурса
        public void ShowPropertiesResourse()
        {
            ResourceTypes type = project.Processes[current_process].Resources[current_block].Type; //берем нужный ресурс из списка
            int id = project.Processes[current_process].Resources[current_block].id;
            List<string> infoResource = dataBase.GetInfoRow(type, id); //берем инфу из базы о нужном ресурсе

            switch (type)
            {
                case ResourceTypes.WORKER: ShowWorker(id, infoResource);
                    break;
                case ResourceTypes.CAD_SYSTEM: ShowCAD(id, infoResource);
                    break;
                case ResourceTypes.TECHNICAL_SUPPORT: ShowTech(id, infoResource);
                    break;
                case ResourceTypes.METHODOLOGICAL_SUPPORT: ShowMethodSupp(id, infoResource);
                    break;
            }
        }

        //свойства исполнителя
        private void ShowWorker(int id, List<string> info)
        {
            GridLength gridHeight = new GridLength(text_height, GridUnitType.Pixel);  //установка высоты ячеек
            //создание таблицы
            ColumnDefinition column1 = new ColumnDefinition();  //добавляем столбцы
            ColumnDefinition column2 = new ColumnDefinition();
            grid_properties.ColumnDefinitions.Add(column1);
            grid_properties.ColumnDefinitions.Add(column2);

            RowDefinition row_fio = new RowDefinition();  //добавляем строки
            RowDefinition row_pos = new RowDefinition();
            RowDefinition row_qualif = new RowDefinition();
            grid_properties.RowDefinitions.Add(row_fio);
            grid_properties.RowDefinitions.Add(row_pos);
            grid_properties.RowDefinitions.Add(row_qualif);

            TextBlock header_fio = CreateTextBlock("Имя", 1);
            Grid.SetColumn(header_fio, 0);
            Grid.SetRow(header_fio, 0);
            grid_properties.Children.Add(header_fio);

            TextBlock header_pos = CreateTextBlock("Должность", 1);
            Grid.SetColumn(header_pos, 0);
            Grid.SetRow(header_pos, 1);
            grid_properties.Children.Add(header_pos);

            TextBlock header_qualif = CreateTextBlock("Квалификация", 1);
            Grid.SetColumn(header_qualif, 0);
            Grid.SetRow(header_qualif, 2);
            grid_properties.Children.Add(header_qualif);

            //заполняем таблицу//
            TextBlock text_fio = CreateTextBlock(info[0], 2);
            Grid.SetColumn(text_fio, 1);
            Grid.SetRow(text_fio, 0);
            grid_properties.Children.Add(text_fio);

            TextBlock text_pos = CreateTextBlock(info[1], 2);
            Grid.SetColumn(text_pos, 1);
            Grid.SetRow(text_pos, 1);
            grid_properties.Children.Add(text_pos);

            TextBlock text_qualif = CreateTextBlock(info[2], 2);
            Grid.SetColumn(text_qualif, 1);
            Grid.SetRow(text_qualif, 2);
            grid_properties.Children.Add(text_qualif);
        }

        //свойства САПР
        private void ShowCAD(int id, List<string> info)
        {
            GridLength gridHeight = new GridLength(text_height, GridUnitType.Pixel);  //установка высоты ячеек
            //создание таблицы
            ColumnDefinition column1 = new ColumnDefinition();
            ColumnDefinition column2 = new ColumnDefinition();
            grid_properties.ColumnDefinitions.Add(column1);
            grid_properties.ColumnDefinitions.Add(column2);

            RowDefinition row_name = new RowDefinition();
            RowDefinition row_form = new RowDefinition();
            RowDefinition row_status = new RowDefinition();
            grid_properties.RowDefinitions.Add(row_name);
            grid_properties.RowDefinitions.Add(row_form);
            grid_properties.RowDefinitions.Add(row_status);

            TextBlock header_name = CreateTextBlock("Название", 1);
            Grid.SetColumn(header_name, 0);
            Grid.SetRow(header_name, 0);
            grid_properties.Children.Add(header_name);

            TextBlock header_form = CreateTextBlock("Форма лицензии", 1);
            Grid.SetColumn(header_form, 0);
            Grid.SetRow(header_form, 1);
            grid_properties.Children.Add(header_form);

            TextBlock header_status;
            if (info[1] == "Full Package Product")
                header_status = CreateTextBlock("Число лицензий, шт", 1);
            else if (info[1] == "Subscription")
                header_status = CreateTextBlock("Оплаченный период, мес", 1);
            else
                header_status = CreateTextBlock("-", 1);
            Grid.SetColumn(header_status, 0);
            Grid.SetRow(header_status, 2);
            grid_properties.Children.Add(header_status);

            //заполняем таблицу//
            TextBlock text_name = CreateTextBlock(info[0], 2);
            Grid.SetColumn(text_name, 1);
            Grid.SetRow(text_name, 0);
            grid_properties.Children.Add(text_name);

            TextBlock text_form = CreateTextBlock(info[1], 2);
            Grid.SetColumn(text_form, 1);
            Grid.SetRow(text_form, 1);
            grid_properties.Children.Add(text_form);

            TextBlock text_status = CreateTextBlock(info[2], 2);
            Grid.SetColumn(text_status, 1);
            Grid.SetRow(text_status, 2);
            grid_properties.Children.Add(text_status);
        }

        //свойства компа
        private void ShowTech(int id, List<string> info)
        {
            GridLength gridHeight = new GridLength(text_height, GridUnitType.Pixel);  //установка высоты ячеек
            //создание таблицы
            ColumnDefinition column1 = new ColumnDefinition();
            ColumnDefinition column2 = new ColumnDefinition();
            grid_properties.ColumnDefinitions.Add(column1);
            grid_properties.ColumnDefinitions.Add(column2);

            RowDefinition row_name = new RowDefinition();
            RowDefinition row_GHz = new RowDefinition();
            RowDefinition row_proc_memory = new RowDefinition();
            RowDefinition row_diagonal = new RowDefinition();
            RowDefinition row_video_memory = new RowDefinition();
            grid_properties.RowDefinitions.Add(row_name);
            grid_properties.RowDefinitions.Add(row_GHz);
            grid_properties.RowDefinitions.Add(row_proc_memory);
            grid_properties.RowDefinitions.Add(row_diagonal);
            grid_properties.RowDefinitions.Add(row_video_memory);

            TextBlock header_name = CreateTextBlock("Имя", 1);
            Grid.SetColumn(header_name, 0);
            Grid.SetRow(header_name, 0);
            grid_properties.Children.Add(header_name);

            TextBlock header_GHz = CreateTextBlock("Частота процессора, ГГц", 1);
            Grid.SetColumn(header_GHz, 0);
            Grid.SetRow(header_GHz, 1);
            grid_properties.Children.Add(header_GHz);

            TextBlock header_proc_memory = CreateTextBlock("Оперативная память, Гб", 1);
            Grid.SetColumn(header_proc_memory, 0);
            Grid.SetRow(header_proc_memory, 2);
            grid_properties.Children.Add(header_proc_memory);

            TextBlock header_diagonal = CreateTextBlock("Диагональ монитора", 1);
            Grid.SetColumn(header_diagonal, 0);
            Grid.SetRow(header_diagonal, 3);
            grid_properties.Children.Add(header_diagonal);

            TextBlock header_video = CreateTextBlock("Память видеокарты, Мб", 1);
            Grid.SetColumn(header_video, 0);
            Grid.SetRow(header_video, 4);
            grid_properties.Children.Add(header_video);

            //заполняем таблицу//
            TextBlock text_name = CreateTextBlock(info[0], 2);
            Grid.SetColumn(text_name, 1);
            Grid.SetRow(text_name, 0);
            grid_properties.Children.Add(text_name);

            TextBlock text_GHz = CreateTextBlock(info[1], 2);
            Grid.SetColumn(text_GHz, 1);
            Grid.SetRow(text_GHz, 1);
            grid_properties.Children.Add(text_GHz);

            TextBlock text_proc_memory = CreateTextBlock(info[2], 2);
            Grid.SetColumn(text_proc_memory, 1);
            Grid.SetRow(text_proc_memory, 2);
            grid_properties.Children.Add(text_proc_memory);

            TextBlock text_diagonal = CreateTextBlock(info[3], 2);
            Grid.SetColumn(text_diagonal, 1);
            Grid.SetRow(text_diagonal, 3);
            grid_properties.Children.Add(text_diagonal);

            TextBlock text_video = CreateTextBlock(info[4], 2);
            Grid.SetColumn(text_video, 1);
            Grid.SetRow(text_video, 4);
            grid_properties.Children.Add(text_video);
        }

        //свойства методологического обеспечения
        private void ShowMethodSupp(int id, List<string> info)
        {
            GridLength gridHeight = new GridLength(text_height, GridUnitType.Pixel);  //установка высоты ячеек
            //создание таблицы
            ColumnDefinition column1 = new ColumnDefinition();  //добавляем столбцы
            ColumnDefinition column2 = new ColumnDefinition();
            grid_properties.ColumnDefinitions.Add(column1);
            grid_properties.ColumnDefinitions.Add(column2);

            RowDefinition row_type = new RowDefinition();  //добавляем строки
            RowDefinition row_multiuse = new RowDefinition();
            grid_properties.RowDefinitions.Add(row_type);
            grid_properties.RowDefinitions.Add(row_multiuse);

            TextBlock header_type = CreateTextBlock("Тип документа", 1);
            Grid.SetColumn(header_type, 0);
            Grid.SetRow(header_type, 0);
            grid_properties.Children.Add(header_type);

            TextBlock header_multiuse = CreateTextBlock("Разделяемый", 1);
            Grid.SetColumn(header_multiuse, 0);
            Grid.SetRow(header_multiuse, 1);
            grid_properties.Children.Add(header_multiuse);

            //заполняем таблицу//
            TextBlock text_type = CreateTextBlock(info[0], 2);
            Grid.SetColumn(text_type, 1);
            Grid.SetRow(text_type, 0);
            grid_properties.Children.Add(text_type);

            TextBlock text_multiuse = CreateTextBlock(info[1], 2);
            Grid.SetColumn(text_multiuse, 1);
            Grid.SetRow(text_multiuse, 1);
            grid_properties.Children.Add(text_multiuse);
        }
    }
}
