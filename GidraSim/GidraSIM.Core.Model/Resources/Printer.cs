using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(IsReference = true)]
    public enum TypePrinter
    {
        Плоттер,
        Принтер,
        Сканер,
        МФУ, //сканер+принте+копир
    }

    [DataContract(IsReference = true)]
    public class Printer : AbstractResource
    {
        [DataMember]
        private byte _speed;

        //public virtual short PrinterId { get; set; }

        public virtual byte Speed 
        {
            get { return _speed; }
            set
            {   
                if(value>0)
                    _speed = value;
                else
                {
                    throw new Exception("Значение Speed не может быть отрицательным");
                }
            }
        }

        [DataMember]
        public TypePrinter Type { get; set; }

        //public IEnumerable<TechnicalSupport> TechnicalSupports { get; set; }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is Printer))
                return false;

            var temp = obj as Printer;

            if (temp.Speed != this.Speed) return false;
            if (temp.Type != this.Type) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
