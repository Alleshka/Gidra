using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

using CommonData;

namespace GidraSIM
{
    /// <summary>
    /// Логика взаимодействия для ModelingParameters.xaml
    /// </summary>
    public partial class ModelingParameters : Window
    {
        List<string> AllComponents; //список типовых компонентов
        ModelingProperties properties;

        public ModelingParameters(ref ModelingProperties parameters)
        {
            InitializeComponent();
            properties = parameters;
            AllComponents = new List<string>();
            //предыдущие параметры моделирования

            textBox_BoardSquare.Text=Convert.ToString(properties.board_square);
            textBox_SquareElements.Text=Convert.ToString(properties.elements_square);
            textBox_Layers.Text=Convert.ToString(properties.layers);

            //заполняем cписок эелементов
            textBox_TwoPolus.Text=Convert.ToString(properties.elements[0].quantity_elements);
            textBox_ThreePolus.Text=Convert.ToString(properties.elements[1].quantity_elements);
            textbox_Chip.Text=Convert.ToString(properties.elements[2].quantity_elements);
            textbox_Other.Text=Convert.ToString(properties.elements[3].quantity_elements);

            FillLists();     		       
        }

        //заполнение списков
        private void FillLists()
        {
            AllComponents.Add("Двухполюсники");  //2 вывода
            AllComponents.Add("Трехполюсники"); //2 вывода
            AllComponents.Add("Микросхемы");   //число выводов назначается пользователем
            AllComponents.Add("Прочее"); //число выводов назначается пользователем
        }

        private void button_Save_Click(object sender, RoutedEventArgs e)// сохраняем все параметры, кроме списка с количеством элементов,
        {                                                               // потому что количество элементов сохраняются при изменении знаычения
            properties.board_square = Convert.ToInt32(textBox_BoardSquare.Text);
            properties.elements_square = Convert.ToInt32(textBox_SquareElements.Text);
            properties.layers = Convert.ToInt32(textBox_Layers.Text);

	//заполняем cписок эелементов
            properties.elements[0].type_element = ComponentsTypes.TWO_POLUS;
            properties.elements[0].quantity_elements = Convert.ToInt32(textBox_TwoPolus.Text);
            properties.elements[0].quantity_pins_min = 2;
            properties.elements[0].quantity_pins_max = 2;

            properties.elements[1].type_element = ComponentsTypes.THREE_POLUS;
            properties.elements[1].quantity_elements = Convert.ToInt32(textBox_ThreePolus.Text);
            properties.elements[1].quantity_pins_min = 3;
            properties.elements[1].quantity_pins_max = 3;

            properties.elements[2].type_element = ComponentsTypes.CHIP;
            properties.elements[2].quantity_elements = Convert.ToInt32(textbox_Chip.Text);

            properties.elements[3].type_element = ComponentsTypes.OTHER;
            properties.elements[3].quantity_elements = Convert.ToInt32(textbox_Other.Text);

            if (properties.elements[2].quantity_elements > 0 || properties.elements[3].quantity_elements > 0)
            {
                QuantityPins window = new QuantityPins(ref properties.elements);
                window.ShowDialog();
            }
            this.Close();
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
