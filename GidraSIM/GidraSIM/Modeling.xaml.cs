using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using CommonData;

namespace GidraSIM
{
    /// <summary>
    /// Логика взаимодействия для Modeling.xaml
    /// </summary>
    public partial class Modeling : Window
    {
        Project project;          //текущий проект
        Process_ current_process; //текущий процесс
        Imitation imitation;      //имитационное моделирование
        int number;               //номер текущего процесса   
        public bool stop;                //остановка выполнения моделирования

        public Modeling(ref Project new_project, int process_number)
        {
            InitializeComponent();

            stop = false;
            project = new_project;
            number = process_number;
            current_process = project.Processes[number];
            ShowResults();
        }
        
        private void ShowResults()
        {
            imitation = new Imitation(ref project, number);//запускаем имитацию
            if (!imitation.RunImitation())
            {
                stop = true;
                this.Close();
            }
            else
            {
                ConvertTimeUnits convert = new ConvertTimeUnits();
                //вывод времени в удобном виде
                label_hole_time.Content = current_process.Time_in_format;
                label_accidents_time.Content = current_process.Time_accidents_in_format;

                label_project_name.Content = project.NameProject;
                label_process_name.Content = current_process.Name;
                label_Complexity.Content = imitation.Ling_Complexity + " (" + Math.Round(imitation.Complexity * 100, 4) + "%)";

                dataGrid_results.Columns.Add(new DataGridTextColumn() { Header = "Имя", Binding = new Binding("Name") });
                dataGrid_results.Columns.Add(new DataGridTextColumn() { Header = "Время выполнения", Binding = new Binding("Time_in_format") });

                List<ModelingResults> results = new List<ModelingResults>();
                results = imitation.getResults();
                dataGrid_results.ItemsSource = results;

                string accident;
                for (int i = 0; i < imitation.accidents_process.Count; i++)
                {
                    if (imitation.accidents_process[i] != "Человеческий фактор: ")
                    {
                        accident = imitation.accidents_process[i] + ": " + imitation.accidents_time_process[i];
                        listBox_accidents.Items.Add(accident);
                    }
                }
                if (imitation.time_between_procedures > 0)
                {
                    accident = "Человеческий фактор: " + convert.DoFullFormat(imitation.time_between_procedures);
                    listBox_accidents.Items.Add(accident);
                }
            }
        }

        //сохранить результаты в базу данных
        private void button_Save_to_base_Click(object sender, RoutedEventArgs e)
        {
            DataBase_ModelingSession modelingSession = new DataBase_ModelingSession(project, number);
            modelingSession.SaveToBase(number);
        }

        //закрыть окно и вернуться в главное
        private void button_Out_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
