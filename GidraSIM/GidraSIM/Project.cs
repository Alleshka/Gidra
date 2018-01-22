using System.Collections.Generic;
using System.Runtime.Serialization;

using CommonData;

namespace GidraSIM
{
    [DataContract]
    public class Project
    {
        [DataMember(EmitDefaultValue = false)]
        public string NameProject;      //имя проекта
        [DataMember(EmitDefaultValue = false)]
        public string WayProject;   //путь к файлу проекта
        [DataMember(EmitDefaultValue = false)]
        public List<Process_> Processes;//список процессов проекта
        [DataMember(EmitDefaultValue = false)]
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
