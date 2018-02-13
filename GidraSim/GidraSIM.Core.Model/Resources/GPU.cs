using System;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract (IsReference = true)]
    public class GPU : AbstractResource
    {
        [DataMember]
        private short _frequency;

        [DataMember]
        private short _memory;

        //public virtual short GpuId { get; set; }

        public virtual short Frequency
        {
            get { return _frequency; }
            set
            {
                if(value>=100&& value<=2000)
                    _frequency = value;
                else
                {
                    throw new Exception("Значение Frequency должно входить в диапазон от 100 до 2000");
                }
            }
        }

        public virtual short Memory 
        {
            get { return _memory; }
            set
            {
                if(value>=64&&value<=12288)
                    _memory = value;
                else
                {
                    throw new Exception("Значение Memory должно входить в диапазон от 64 до 12288");
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is GPU))
                return false;

            var temp = obj as GPU;

            if (temp.Frequency != this.Frequency) return false;
            if (temp.Memory != this.Memory) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
