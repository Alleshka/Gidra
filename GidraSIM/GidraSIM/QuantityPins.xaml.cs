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

using CommonData;
namespace GidraSIM
{
    /// <summary>
    /// Логика взаимодействия для QuantityPins.xaml
    /// </summary>
    public partial class QuantityPins : Window
    {
        ComponentsParameters [] components;

        public QuantityPins(ref ComponentsParameters [] new_components)
        {
            InitializeComponent();
            components = new_components;
            
            //делаем неактивными то, чего нет на плате
            if (components[2].quantity_elements == 0)// микросхемы
            {
                textBox_minPins_Chips.IsEnabled = false;
                textBox_maxPins_Chips.IsEnabled = false;
                textBox_OftenPins_Chips.IsEnabled = false;
            }
            else //выводим предыдущие значения
            {
                textBox_minPins_Chips.Text=Convert.ToString(components[2].quantity_pins_min);
                textBox_maxPins_Chips.Text=Convert.ToString(components[2].quantity_pins_max);
                textBox_OftenPins_Chips.Text = Convert.ToString(components[2].quantity_pins_often);
            }

            if (components[3].quantity_elements == 0)//другие элемениы
            {
                textBox_minPins_Other.IsEnabled = false;
                textBox_maxPins_Other.IsEnabled = false;
                textBox_OftenPins_Other.IsEnabled = false;
            }
            else //выводим предыдущие значения
            {
                textBox_minPins_Other.Text = Convert.ToString(components[3].quantity_pins_min);
                textBox_maxPins_Other.Text = Convert.ToString(components[3].quantity_pins_max);
                textBox_OftenPins_Other.Text = Convert.ToString(components[3].quantity_pins_often);
            }
        }

        private void button_Save_Click(object sender, RoutedEventArgs e)
        {
            bool ok = true;
            components[2].quantity_pins_min = Convert.ToInt32(textBox_minPins_Chips.Text);
            components[2].quantity_pins_max = Convert.ToInt32(textBox_maxPins_Chips.Text);
            components[2].quantity_pins_often = Convert.ToInt32(textBox_OftenPins_Chips.Text);

            components[3].quantity_pins_min = Convert.ToInt32(textBox_minPins_Other.Text);
            components[3].quantity_pins_max = Convert.ToInt32(textBox_maxPins_Other.Text);
            components[3].quantity_pins_often = Convert.ToInt32(textBox_OftenPins_Other.Text);

            if (components[2].quantity_elements > 0)
                if (components[2].quantity_pins_min < 1 || components[2].quantity_pins_max < 1)
                    ok = false;

            if (components[3].quantity_elements > 0)
                if (components[3].quantity_pins_min < 1 || components[3].quantity_pins_max < 1)
                    ok = false;
            
            if (ok) 
                this.Close();
            else
                MessageBox.Show("Число выводов должно быть больше 1", "Вот и неправильно");
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
