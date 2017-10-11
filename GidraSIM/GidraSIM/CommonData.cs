using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.Serialization;

namespace CommonData   //содержит общие структуры и данные
{
    [DataContract]

    public enum Size_Block
    {
        WIDTH_BLOCK = 100,
        HEIGHT_BLOCK = 100,
        SIZE_BLOCK_BE = 50  // размеры для блоков бегина и энда
    }

    public enum ObjectTypes //типы объектов процесса
    {
        CURSOR,
        IMAGE,
        PROCEDURE,
        RESOURCE,
        CONNECT,
        PARALLEL_PROCESS,
        SUBPROCESS,
        BEGIN,
        END,
        NO_OBJECT
    }

    public enum ComponentsTypes //компоненты ПП
    {
        TWO_POLUS,  //двухполюсники
        THREE_POLUS,//трехполюсники
        CHIP,       //микросхема
        OTHER       //другое
    }

    public enum ResourceTypes //типы ресурсов
    {
        WORKER,
        CAD_SYSTEM,
        TECHNICAL_SUPPORT,
        METHODOLOGICAL_SUPPORT,
        NO_TYPE
    }

    [DataContract]
    public class Connection_Line    //линия связи между блоками
    {
        [DataMember]
        public int block1;
        [DataMember]
        public int block2;

        public Line object_line;

        public Connection_Line(int number1, int number2, Line new_line)
        {
            block1 = number1;  //номера блоков в списке images_in_tabItem
            block2 = number2;
            object_line = new_line;
        }
    }

    [DataContract]
    public class Neibour    //сосед объекта
    {
        [DataMember]
        public ObjectTypes type;   //его тип
        [DataMember]
        public int number;         //номер в списке соответствующих объектов

        public Neibour(ObjectTypes new_type, int new_number)
        {
            type = new_type;
            number = new_number;
        }
    }

    [DataContract]
    public class StructureObject   //структура объекта процесса
    {
        [DataMember]
        public ObjectTypes Type;
        [DataMember]
        public int number;        //номер объекта в списке соотвветствующих объектов
        [DataMember]
        public Point point;       // координаты середины блока
    }

    [DataContract]
    public class BlockObject  //структура графического блока
    {
        public Image image;  //картинка блока
        public TextBlock label;  //надпись в блоке 
        [DataMember]
        public StructureObject object_of_block;   //инф-я о блоке
    }

    [DataContract]
    public class ComponentsParameters  //параметры компонентов платы
    {
        [DataMember]
        public int quantity_elements;         //число элементов конкретного типа
        [DataMember]
        public ComponentsTypes type_element;  //тип элемента
        [DataMember]
        public int quantity_pins_min;       //макс и мин число выводов у микросхем и прочего
        [DataMember]
        public int quantity_pins_max;
        [DataMember]
        public int quantity_pins_often;              //наиболее часто встречающееся количество выводов у элемента
    }

    [DataContract]
    public class ModelingProperties  //параметры моделирования (объекта проектирования)
    {
        [DataMember]
        public ComponentsParameters[] elements;  //список параметров компонентов
        [DataMember]
        public double board_square;                  //площадь платы
        [DataMember]
        public double elements_square;               //общая площадь элементов
        [DataMember]
        public int layers;                           //число слоев
       
        public ModelingProperties()
        {
            elements = new ComponentsParameters[4];
            elements[0] = new ComponentsParameters();
            elements[1] = new ComponentsParameters();
            elements[2] = new ComponentsParameters();
            elements[3] = new ComponentsParameters();
            board_square = elements_square = layers = 0;
        }
    }

    public class SqlServer//изменение имени сервера
    {
        public string GetServerName()
        {
           //return "USER-ПК";
              return "(localdb)\\MSSQLLocalDB";
        }
    }

    public class ModelingResults
    {
        public string Name {get; set;}
        public string Time_in_format {get; set;}
    }

}
