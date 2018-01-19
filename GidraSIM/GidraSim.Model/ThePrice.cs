using System;

namespace GidraSim.Model
{
    public abstract class ThePrice
    {
        private decimal _price;

        public virtual decimal Price
        {
            get { return _price; }
            set
            {
                if (value > 0)
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
