using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract (IsReference = true)]
    public class Monitor : AbstractResource
    {
        [DataMember]
        private byte _diagonal;

        //public virtual short MonitorId { get; set; }

        public virtual byte Diagonal
        {
            get { return _diagonal; }
            set
            {
                if(value>=4&&value<=54)
                    _diagonal = value;
                else
                {
                    throw new Exception("Значение Diagonal должно входить в диапазон от 4 до 54");
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is Monitor))
                return false;

            var temp = obj as Monitor;

            if (temp.Diagonal != this.Diagonal) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
