using System;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract (IsReference = true)]
    public class CPU : AbstractResource
    {
        [DataMember]
        private byte _quantityCore;
        [DataMember]
        private short _frequency;

        //public virtual short CpuId { get; set; }

        public virtual byte QuantityCore
        {
            get { return _quantityCore; }
            set
            {
                if(value>=1&&value<=24)
                    _quantityCore = value;
                else
                {
                    throw new Exception("Значение QuantityCore должно входить в диапазон от 1 до 24");
                }
            }
        }

        public virtual short Frequency  
        {
            get { return _frequency; }
            set
            {
                if(value>=500&&value<=16000)
                    _frequency = value;
                else
                {
                    throw new Exception("Значение Frequency должно входить в диапазон от 500 до 16000");
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is CPU))
                return false;

            var temp = obj as CPU;

            if (temp.QuantityCore != this.QuantityCore) return false;
            if (temp.Frequency != this.Frequency) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
