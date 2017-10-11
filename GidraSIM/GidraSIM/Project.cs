using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using CommonData;

namespace GidraSIM
{
    [DataContract]
    public class Project
    {
        [DataMember]
        public string NameProject;      //имя проекта
        [DataMember]
        public string WayProject;   //путь к файлу проекта
        [DataMember]
        public List<Process_> Processes;//список процессов проекта
        [DataMember]
        public ModelingProperties modelingProperties; //параметры моделирования (объекта проектирования)

        //  public List<List<BlockObject>> images_in_tabControl;    //список блоков во всех вкладках

        public Project(string name, string way)
        {
            NameProject = name;
            WayProject = way;
            Processes = new List<Process_>();
            modelingProperties = new ModelingProperties();
        }
    }
}
