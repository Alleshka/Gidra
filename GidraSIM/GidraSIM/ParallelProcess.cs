using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using CommonData;


namespace GidraSIM
{
    [DataContract]
    public class ParallelProcess
    {
        [DataMember]
        int CountBranches;         //число параллельных ветвей
        [DataMember]
        Neibour Left_Neibour;      //левый сосед
        [DataMember]
        Neibour Right_Neibour;     //правый сосед        
        [DataMember]
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
