using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;

using CommonData;

namespace GidraSIM
{
    class RecreateProjectFromOpened
    {
        Project project;
        DrawShema drawing;
        TabControl TabControlMain;

        public RecreateProjectFromOpened(ref Project project_to_recreate,  ref TabControl tab_control)
        {
            project = project_to_recreate;
            TabControlMain = tab_control;
            drawing = new DrawShema();

            RecreateImagesInTabItem();//Воссоздаем блоки
            RecreateConnactionLines();//Воссоздаем линии
        }

//Воссоздаем линии
        public void RecreateConnactionLines()
        {
            for (int num_proc = 0; num_proc < project.Processes.Count; num_proc++)//идем по процессам
            {
                List<Connection_Line> lines_local = new List<Connection_Line>();//создаем локальный список линий
                TabItem currentTab = TabControlMain.Items[num_proc] as TabItem;
                Canvas canvas_process = currentTab.Content as Canvas;
                for (int num_line = 0; num_line < project.Processes[num_proc].connection_lines.Count; num_line++)//идем по линиям
                {
                    if (project.Processes[num_proc].images_in_tabItem[project.Processes[num_proc].connection_lines[num_line].block1].object_of_block.Type == ObjectTypes.BEGIN ||
                        project.Processes[num_proc].images_in_tabItem[project.Processes[num_proc].connection_lines[num_line].block2].object_of_block.Type == ObjectTypes.BEGIN)
                        drawing.SetLight(true);
                    Point p1 = project.Processes[num_proc].images_in_tabItem[project.Processes[num_proc].connection_lines[num_line].block1].object_of_block.point;//берем коордитнаты блока
                    Point p2 = project.Processes[num_proc].images_in_tabItem[project.Processes[num_proc].connection_lines[num_line].block2].object_of_block.point;//берем коордитнаты блока
                    Connection_Line connection = new Connection_Line(project.Processes[num_proc].connection_lines[num_line].block1,
                                                                     project.Processes[num_proc].connection_lines[num_line].block2,
                                                                     drawing.DrawLine(p1, p2));
                    lines_local.Add(connection);
                }
                project.Processes[num_proc].connection_lines = lines_local;
                for (int i = 0; i < project.Processes[num_proc].connection_lines.Count; i++)
                    canvas_process.Children.Add(project.Processes[num_proc].connection_lines[i].object_line);
                currentTab.Content = canvas_process;
            }
        }

//Воссоздаем блоки
        public void RecreateImagesInTabItem()
        {
            for (int num_proc = 0; num_proc < project.Processes.Count; num_proc++)//идем по процессам
            {
                List<BlockObject> images_in_tabItem_local = new List<BlockObject>();
                Canvas canvas_process = new Canvas(); //новая канва для рисования процесса   
                TabItem currentTab = new TabItem();    //создаем новую вкладку
                TabControlMain.Items.Add(currentTab); //добавили новую вкладку 
                currentTab = TabControlMain.Items[num_proc] as TabItem; //берем только что добавленную вкладку 
                currentTab.Header = new TextBlock { Text = project.Processes[num_proc].Name };
                drawing.ChangeCanva(canvas_process, ref images_in_tabItem_local);// смена канвы
                drawing.ChangeImagesInTabItem(ref images_in_tabItem_local);
                for (int num_struct = 0; num_struct < project.Processes[num_proc].images_in_tabItem.Count; num_struct++)//идем по структуре
                {
                    if (project.Processes[num_proc].images_in_tabItem[num_struct].object_of_block.Type == ObjectTypes.BEGIN)
                        drawing.AddBeginEnd(TabControlMain.Width, TabControlMain.Height);
                    else if (project.Processes[num_proc].images_in_tabItem[num_struct].object_of_block.Type != ObjectTypes.BEGIN &&
                             project.Processes[num_proc].images_in_tabItem[num_struct].object_of_block.Type != ObjectTypes.END)  ///рисуем все блоки, кроме бегина и энда
                        drawing.AddBlock(project.Processes[num_proc].images_in_tabItem[num_struct].object_of_block.Type,
                                         project.Processes[num_proc].images_in_tabItem[num_struct].object_of_block.point,
                                         project.Processes[num_proc].images_in_tabItem[num_struct].object_of_block.number,"");
                }
                project.Processes[num_proc].images_in_tabItem = images_in_tabItem_local;
                for (int i = 0; i < project.Processes[num_proc].images_in_tabItem.Count; i++)
                {
                    if (project.Processes[num_proc].images_in_tabItem[i].object_of_block.Type == ObjectTypes.PROCEDURE)
                        project.Processes[num_proc].images_in_tabItem[i].label.Text = project.Processes[num_proc].Procedures[project.Processes[num_proc].images_in_tabItem[i].object_of_block.number].Name;
                    else if (project.Processes[num_proc].images_in_tabItem[i].object_of_block.Type == ObjectTypes.RESOURCE)
                        drawing.SetResourceImage(project.Processes[num_proc].Resources[project.Processes[num_proc].images_in_tabItem[i].object_of_block.number].Type,
                                                 project.Processes[num_proc].Resources[project.Processes[num_proc].images_in_tabItem[i].object_of_block.number].id, i);
                    else if (project.Processes[num_proc].images_in_tabItem[i].object_of_block.Type == ObjectTypes.SUBPROCESS)
                    {
                        int number_in_subprocess = project.Processes[num_proc].images_in_tabItem[i].object_of_block.number;
                        int number_in_process=project.Processes[num_proc].SubProcesses[number_in_subprocess].number_in_processes;
                        project.Processes[num_proc].images_in_tabItem[i].label.Text = project.Processes[number_in_process].Name;
                    }

                    canvas_process.Children.Add(project.Processes[num_proc].images_in_tabItem[i].image);
                    canvas_process.Children.Add(project.Processes[num_proc].images_in_tabItem[i].label);                    
                }
                currentTab.Content = canvas_process;
            }
        }
    }
}
