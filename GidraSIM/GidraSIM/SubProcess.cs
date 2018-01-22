using CommonData;
using System.Runtime.Serialization;

namespace GidraSIM
{
    [DataContract]
    public class SubProcess
    {
        [DataMember(EmitDefaultValue = false)]
        public int number_in_processes; //номер этого подпроцесса в списке процессов проекта
        [DataMember(EmitDefaultValue = false)]
        public Neibour Left_Neibour;      //левый сосед
        [DataMember(EmitDefaultValue = false)]
        public Neibour Right_Neibour;     //правый сосед

        public SubProcess()
        {
            Left_Neibour = new Neibour(ObjectTypes.NO_OBJECT, -1);        //по умолчанию соседей нет
            Right_Neibour = new Neibour(ObjectTypes.NO_OBJECT, -1);
        }
    }
}
