using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSim.Model.Resources
{
    public class Monitor:ResurcePrice
    {
        private byte _diagonal;
        public virtual short MonitorId { get; set; }

        public virtual byte Diagonal
        {
            get { return _diagonal; }
            set
            {
                if(value>=4&&value<=54)
                    _diagonal = value;
                else
                {
                    throw new Exception("Значение должно входить в диапазон от 4 до 54");
                }
            }
        }
    }
}
