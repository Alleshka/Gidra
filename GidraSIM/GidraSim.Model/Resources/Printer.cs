using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSim.Model.Resources
{
    public enum TypePrinter
    {
        Плоттер,
        Принтер,
        Сканер,
        МФУ, //сканер+принте+копир
    }

    public class Printer:ThePrice
    {
        private byte _speed;
        public virtual short PrinterId { get; set; }

        public virtual byte Speed 
        {
            get { return _speed; }
            set
            {   
                if(value>0)
                    _speed = value;
                else
                {
                    throw new Exception("Значение не может быть отрицательным");
                }
            }
        }

        public TypePrinter Type { get; set; }

        public IEnumerable<TechnicalSupport> TechnicalSupports { get; set; }    
    }
}
