using System.Collections.Generic;
using System.Windows;

namespace GidraSIM
{
    /// <summary>
    /// Логика взаимодействия для SetSubProcess.xaml
    /// </summary>
    public partial class SetSubProcess : Window
    {
        Project project;
        int current_process;
        public int chosen_process_number;

        public SetSubProcess(Project new_project, int number_process)
        {
            InitializeComponent();
            project = new_project;
            current_process = number_process;
            FillComboBox();
        }

        private void FillComboBox()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < project.Processes.Count; i++)
                if (i != current_process)
                    names.Add(project.Processes[i].Name);
            comboBox_SelectProcess.ItemsSource = names;
            comboBox_SelectProcess.SelectedIndex = 0;
        }

        private void button_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_SelectProcess.SelectedIndex >= current_process)
                chosen_process_number = comboBox_SelectProcess.SelectedIndex + 1;
            else
                chosen_process_number = comboBox_SelectProcess.SelectedIndex;
            this.Close();
        }
    }
}
