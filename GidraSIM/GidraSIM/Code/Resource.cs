using CommonData;
using System.Runtime.Serialization;

namespace GidraSIM
{
    [DataContract]
    public class Resource
    {
        [DataMember]
        public int id;                    //id строки в таблице ресурса указанного типа type
        [DataMember]
        public ResourceTypes Type;        //тип ресурса

        public Resource()
        {
            Type = ResourceTypes.NO_TYPE;                 //по умолчанию тип не определен
        }
    }
}
