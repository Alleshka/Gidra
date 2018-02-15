using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model
{
    public class AccidentsCollector : IAccidentsCollector, IObjectReference
    {
        [DataMember(EmitDefaultValue = false)]
        private List<Accident> accidents;

        private static AccidentsCollector collector = new AccidentsCollector();

        private AccidentsCollector()
        {
            accidents = new List<Accident>();
        }

        public static AccidentsCollector GetInstance()
        {
            return collector;
        }

        public void Collect(Accident accident)
        {
            accidents.Add(accident);
        }

        public List<Accident> GetHistory()
        {
            return accidents;
        }

        // Это надо для нормальной сериализации синглтона 

        public object GetRealObject(StreamingContext context)
        {
            AccidentsCollector realObject = GetInstance();
            realObject.Merge(this);
            return realObject;
        }

        private void Merge(AccidentsCollector otherInstance)
        {
            this.accidents = otherInstance.accidents;
        }
    }
}
