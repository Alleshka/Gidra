using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSim.Model.Resources
{
    public class Gpu:ThePrice
    {
        private short _frequency;
        private short _memory;
        public virtual short GpuId { get; set; }

        public virtual short Frequency
        {
            get { return _frequency; }
            set
            {
                if(value>=100&& value<=2000)
                    _frequency = value;
                else
                {
                    throw new Exception("Значение должно входить в диапазон от 100 до 2000");
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
                    throw new Exception("Значение должно входить в диапазон от 64 до 12288");
                }
            }
        }
    }
}
