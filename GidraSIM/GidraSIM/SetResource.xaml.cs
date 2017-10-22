using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CommonData;

namespace GidraSIM
{
    /// <summary>
    /// Логика взаимодействия для SetResource.xaml
    /// </summary>
    public partial class SetResource : Window
    {
        List<string> types;
        DataBase_Resourses dataBase;
        public ResourceTypes selectedType;
        public int selectedId;

        public SetResource()
        {
            InitializeComponent();
            types = new List<string>(4);
            types.Add("Исполнитель");
            types.Add("САПР-система");
            types.Add("Техническое обеспечение");
            types.Add("Методологическое обеспечение");
            comboBox1.ItemsSource = types;
            selectedId = -1;
            comboBox1.IsEnabled = true;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataBase = new DataBase_Resourses(dataGrid1, comboBox1.SelectedIndex, this);
            dataBase.LoadTable();
            dataBase.ShowTable();
        }

        private void button_SaveResource_Click(object sender, RoutedEventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        selectedId = ((Workers)dataGrid1.SelectedItem).worker_id;  //берем id выделенной строки
                        selectedType = ResourceTypes.WORKER;
                    }
                    break;
                case 1:
                    {
                        selectedId = ((CAD_Systems)dataGrid1.SelectedItem).cad_id;  //берем id выделенной строки
                        selectedType = ResourceTypes.CAD_SYSTEM;
                    }
                    break;
                case 2:
                    {
                        selectedId = ((Technical_Support)dataGrid1.SelectedItem).tech_supp_id;  //берем id выделенной строки
                        selectedType = ResourceTypes.TECHNICAL_SUPPORT;
                    }
                    break;
                case 3:
                    {
                        selectedId = ((Methodological_Support)dataGrid1.SelectedItem).method_supp_id;  //берем id выделенной строки
                        selectedType = ResourceTypes.METHODOLOGICAL_SUPPORT;
                    }
                    break;
            }
            this.Close();
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
