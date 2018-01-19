using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSim.Model.Resources
{
    public class Cpu:ThePrice
    {
        private byte _quantityCore;
        private short _frequency;
        public virtual short CpuId { get; set; }

        public virtual byte QuantityCore
        {
            get { return _quantityCore; }
            set
            {
                if(value>=1&&value<=24)
                    _quantityCore = value;
                else
                {
                    throw new Exception("Значение должно входить в диапазон от 1 до 24");
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
                    throw new Exception("Значение должно входить в диапазон от 500 до 16000");
                }
            }
        }
    }
}
