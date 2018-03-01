using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
using System.ComponentModel;

namespace GidraSIM.Core.Model.Resources
{
    public enum TypeIS
    {
        [Description("Бумажный")]
        Бумажный,
        [Description("Электронный")]
        Электронный
    }

    [DataContract(IsReference = true)]
    public class InformationSupport : AbstractResource
    {
        //public virtual short InformationSupportId { get; set; }

        [DataMember]
        public virtual bool MultiClientUse { get; set; }

        [DataMember]
        public virtual TypeIS Type { get; set; }

        // TODO: ???????????????????????????????????
        //public IEnumerable<Process> Processes { get; set; } 

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is InformationSupport))
                return false;

            var temp = obj as InformationSupport;

            if (temp.MultiClientUse != this.MultiClientUse) return false;
            if (temp.Type != this.Type) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
