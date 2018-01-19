using System;

namespace GidraSim.Model.Resources
{
    public class Ram:ResurcePrice
    {
        private byte _size;
        private short _frequency;
        public virtual short RamId { get; set; }

        public virtual byte Size
        {
            get { return _size; }
            set
            {
                if(value>=1 &&value<=64)
                    _size = value;
                else
                {
                    throw new Exception("Значение должно входить в диапазон от 1 до 64");
                }
            }
        }

        public virtual short Frequency
        {
            get { return _frequency; }
            set
            {
                if(value>=200&&value<=3333)
                    _frequency = value;
                else
                {
                    throw new Exception("Значение должно находится в диапозоне от 200 до 3333");
                }
            }
        }
    }
}
