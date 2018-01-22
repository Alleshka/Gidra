using System.Collections.Generic;
using CommonData;
using System.Runtime.Serialization;

namespace GidraSIM
{
    [DataContract]
    public class Procedure
    {
        [DataMember(EmitDefaultValue = false)]
        public string Name {get; set;}              //имя процедуры
        [DataMember(EmitDefaultValue = false)]
        public bool is_common;              //типовая ли
        [DataMember(EmitDefaultValue = false)]
        public string common_type;      //тип 
        [DataMember(EmitDefaultValue = false)]
        public double Time_in_days {get; set;}                //длительность выполнения процедуры В ДНЯХ
        [DataMember(EmitDefaultValue = false)]
        public string Time_in_format { get; set; }       //длительность выполнения процедуры В читабельном формате
        [DataMember(EmitDefaultValue = false)]
        public Neibour Left_Neibour;      //левый сосед
        [DataMember(EmitDefaultValue = false)]
        public Neibour Right_Neibour;     //правый сосед
        [DataMember(EmitDefaultValue = false)]
        public List<int> Resources;       //список подключенных номеров ресурсов из списка ресурсов (project.Resourses)

        public Procedure()
        {
            Left_Neibour = new Neibour(ObjectTypes.NO_OBJECT, -1);        //по умолчанию соседей нет
            Right_Neibour = new Neibour(ObjectTypes.NO_OBJECT, -1);
            Resources = new List<int>();
        }
    }
}
