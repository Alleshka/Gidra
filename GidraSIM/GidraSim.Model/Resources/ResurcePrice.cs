using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSim.Model.Resources
{
    public abstract class ResurcePrice
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
