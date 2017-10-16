using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GidraSIM
{
    /// <summary>
    /// Логика взаимодействия для SetProcedure.xaml
    /// </summary>
    public partial class SetProcedure : Window
    {
        public List<string> commonProcedures; //список типовых процедур

        Project project;
        Procedure procedure;
        int num_procedure;
        int num_process;        

        public SetProcedure(ref Project new_project, int new_num_procedure, int new_num_process)
        {
            InitializeComponent();
            num_procedure = new_num_procedure;
            num_process = new_num_process;
            project = new_project;
            procedure = project.Processes[num_process].Procedures[num_procedure];
            textBox_NotCommonTime.IsEnabled = false;
            commonProcedures = new List<string>();

            //выводим старые свойства процедуры
            textBox_Name.Text = procedure.Name;
            if (procedure.is_common)//если процедура со считаемым временем
            {
                radioButton_IsCommon.IsChecked = true;
                comboBox_Commons.SelectedValue = procedure.common_type;

            }
            else//если с задающимся
            {
                radioButton_NotCommon.IsChecked = true;
                textBox_NotCommonTime.Text = Convert.ToString(procedure.Time_in_format);
            }
            FillLists();
        }

//заполнение списков
        private void FillLists()  
        {
            commonProcedures.Add("Создание электрической схемы");//0
            commonProcedures.Add("Моделирование электрической схемы");//1
            commonProcedures.Add("Компоновка");//2
            commonProcedures.Add("Размещение");//3
            commonProcedures.Add("Трассировка");//4
            commonProcedures.Add("Оформление документации");//5
            commonProcedures.Add("Согласование документации с контролирующими службами");//6
            commonProcedures.Add("Согласование документации с заказчиком");//7
            commonProcedures.Add("Тестирование опытного образца объекта");//8

            comboBox_Commons.ItemsSource = commonProcedures;
            radioButton_IsCommon.IsChecked = false;
            radioButton_NotCommon.IsChecked = false;
        }

//если выбрали ТПП
        private void radioButton_IsCommon_Checked(object sender, RoutedEventArgs e)
        {
            comboBox_Commons.SelectedIndex = 0;
        }

//если не ТПП
        private void radioButton_NotCommon_Checked(object sender, RoutedEventArgs e)
        {
            textBox_NotCommonTime.IsEnabled = true;
        }        

//сохраняем выбранные значения
        private void button_SaveProcedure_Click(object sender, RoutedEventArgs e)
        {
            procedure.Name = textBox_Name.Text;
            if ((bool)radioButton_IsCommon.IsChecked)
            {
                procedure.is_common = true;
                procedure.common_type = commonProcedures[comboBox_Commons.SelectedIndex];
            }
            else
            {
                ConvertTimeUnits convert = new ConvertTimeUnits();
                procedure.is_common = false;
                procedure.Time_in_days = convert.ConvertToDays(comboBox_TimeUnit.SelectedIndex, Convert.ToDouble(textBox_NotCommonTime.Text));
            }
            project.Processes[num_process].Procedures[num_procedure] = procedure;
            this.Close();
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBox_NotCommonTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            textBox_NotCommonTime.Text="";
        }

        private void textBox_NotCommonTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            radioButton_IsCommon.IsChecked = false;
            radioButton_NotCommon.IsChecked = true;
        }
    }
}
