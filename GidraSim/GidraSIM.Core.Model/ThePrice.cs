using System;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model
{
    [DataContract(IsReference = true)]
    public abstract class ThePrice
    {
        [DataMember]
        private decimal _price;

        public virtual decimal Price
        {
            get { return _price; }
            set
            {
                if (value >= 0)
                {
                    _price = value;
                }
                else
                {
                    throw new Exception("Значение не может быть отрицательным");
                }
            }
        }
    }
}
