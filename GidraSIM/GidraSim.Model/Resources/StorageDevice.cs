using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSim.Model.Resources
{
    public class StorageDevice :ThePrice
    {
        private short _speedWrite;
        private short _speedRead;
        private short _size;
        public virtual short StorageDeviceId { get; set; }

        public virtual short SpeedWrite
        {
            get { return _speedWrite; }
            set
            {
                if(value>0)
                    _speedWrite = value;
                else
                {
                    throw new Exception("Значение не может быть отрицательным");
                }
            }
        }

        public virtual short SpeedRead  
        {
            get { return _speedRead; }
            set
            {
                if(value>0)
                    _speedRead = value;
                else
                {
                    throw new Exception("Значение не может быть отрицательным");
                }
            }
        }

        public virtual short Size   
        {
            get { return _size; }
            set
            {
                if(value>=64&&value<=1024)
                    _size = value;
                else
                {
                    throw new Exception("Значение должно входить в диапазон от 64 до 1024");
                }
            }
        }
    }
}
    