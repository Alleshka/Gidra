using System;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(IsReference = true)]
    public class RAM : AbstractResource
    {
        [DataMember]
        private byte _size;

        [DataMember]
        private short _frequency;
        //public virtual short RamId { get; set; }

        public virtual byte Size
        {
            get { return _size; }
            set
            {
                if(value>=1 &&value<=64)
                    _size = value;
                else
                {
                    throw new Exception("Значение Size должно входить в диапазон от 1 до 64");
                }
            }
        }

        public virtual short Frequency
        {
            get { return _frequency; }
            set
            {
                if(value>=200&&value<=3333)
                    _frequency = value;
                else
                {
                    throw new Exception("Значение Frequency должно находится в диапозоне от 200 до 3333");
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is RAM))
                return false;

            var temp = obj as RAM;

            if (temp.Size != this.Size) return false;
            if (temp.Frequency != this.Frequency) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
