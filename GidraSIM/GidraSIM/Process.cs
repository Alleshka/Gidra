using System.Collections.Generic;
using CommonData;
using System.Runtime.Serialization;

namespace GidraSIM
{
    [DataContract]
    public class Process_
    {
        [DataMember]
        public string Name;                           //имя процесса
        [DataMember]
        public bool Correct;                          //коррекность процесса
        [DataMember]
        public double Time_in_days;                  //время выполнения процесса в днях
        [DataMember]
        public string Time_in_format;                //время выполнения процесса в читабельном формате
        [DataMember]
        public double Time_accidents_in_days;                 //время задержек из-за непредвиденных событий в днях
        [DataMember]
        public string Time_accidents_in_format;                 //время задержек из-за непредвиденных событий в читабельном формате
        [DataMember]
        public int IsSub;                             //является ли процесс вложенным и если да, то куда (номер в списке процессов),
                                                      //-1 - никуда не вложен
        [DataMember]
        public List<StructureObject> StructProcess;   //структура процесса
        [DataMember]
        public List<Procedure> Procedures;           //список процедур
        [DataMember]
        public List<Resource> Resources;             //список ресурсов
        [DataMember]
        public List<SubProcess> SubProcesses;               //список вложенных процессов (их номеров в списке процессов проекта)
        [DataMember]
        public List<ParallelProcess> Parallels;      //список параллельных процессов
        [DataMember]
        public List<Connection_Line> connection_lines;         //список соединительных линий
        [DataMember]
        public List<BlockObject> images_in_tabItem;          //список блоков в конкретной вкладке

        public Process_(string new_Name)
        {
            Name = new_Name;
            Correct = false;
            IsSub = -1;           //по умолчанию - не вложен
            StructProcess = new List<StructureObject>();
            Procedures = new List<Procedure>();
            Resources = new List<Resource>();
            SubProcesses = new List<SubProcess>();
            Parallels = new List<ParallelProcess>();
            images_in_tabItem = new List<BlockObject>();
            connection_lines = new List<Connection_Line>();
        }
    }
}
