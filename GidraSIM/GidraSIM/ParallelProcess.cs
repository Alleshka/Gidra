using System.Collections.Generic;
using System.Runtime.Serialization;

using CommonData;


namespace GidraSIM
{
    [DataContract]
    public class ParallelProcess
    {
        [DataMember(EmitDefaultValue = false)]
        int CountBranches;         //число параллельных ветвей
        [DataMember(EmitDefaultValue = false)]
        Neibour Left_Neibour;      //левый сосед
        [DataMember(EmitDefaultValue = false)]
        Neibour Right_Neibour;     //правый сосед        
        [DataMember(EmitDefaultValue = false)]
        List<Procedure>[] Parallel;//массив ветвей

        public ParallelProcess(int count)
        {
            CountBranches = count;
            //List<Procedure> Branch;    //список процедур в ветке
            Left_Neibour = new Neibour(ObjectTypes.NO_OBJECT, -1);        //по умолчанию соседей нет
            Right_Neibour = new Neibour(ObjectTypes.NO_OBJECT, -1);
            Parallel = new List<Procedure>[CountBranches];
        }
    }
}
