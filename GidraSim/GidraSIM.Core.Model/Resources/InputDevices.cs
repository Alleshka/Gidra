using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract]
    public enum TypeInputDevices
    {
        [Description("Клавиатура")]
        KeyBoard,
        [Description("Мышка")]
        Mouse
    }

    [DataContract]
    public class InputDevices : AbstractResource
    {
        //public virtual short InputDevicesId { get; set; }

        [DataMember]
        public virtual TypeInputDevices Type { get; set; }

        // TODO: Это что??? Надо ли это????
        //public IEnumerable<TechnicalSupport> TechnicalSupports { get; set; }    

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is InputDevices))
                return false;

            var temp = obj as InputDevices;

            if (temp.Type != this.Type) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
