using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonData;
using System.Windows;
using System.Runtime.Serialization;

namespace GidraSIM
{
    [DataContract]
    public class Procedure
    {
        [DataMember]
        public string Name {get; set;}              //имя процедуры
        [DataMember]
        public bool is_common;              //типовая ли
        [DataMember]
        public string common_type;      //тип 
        [DataMember]
        public double Time_in_days {get; set;}                //длительность выполнения процедуры В ДНЯХ
        [DataMember]
        public string Time_in_format { get; set; }       //длительность выполнения процедуры В читабельном формате
        [DataMember]
        public Neibour Left_Neibour;      //левый сосед
        [DataMember]
        public Neibour Right_Neibour;     //правый сосед
        [DataMember]
        public List<int> Resources;       //список подключенных номеров ресурсов из списка ресурсов (project.Resourses)

        public Procedure()
        {
            Left_Neibour = new Neibour(ObjectTypes.NO_OBJECT, -1);        //по умолчанию соседей нет
            Right_Neibour = new Neibour(ObjectTypes.NO_OBJECT, -1);
            Resources = new List<int>();
        }
    }
}
