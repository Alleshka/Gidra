using System;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(IsReference = true)]
    public class StorageDevice : AbstractResource
    {
        [DataMember]
        private short _speedWrite;
        [DataMember]
        private short _speedRead;
        [DataMember]
        private short _size;

        //public virtual short StorageDeviceId { get; set; }

        public virtual short SpeedWrite
        {
            get { return _speedWrite; }
            set
            {
                if(value>0)
                    _speedWrite = value;
                else
                {
                    throw new Exception("Значение SpeedWrite не может быть отрицательным");
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
                    throw new Exception("Значение SpeedRead не может быть отрицательным");
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
                    throw new Exception("Значение Size должно входить в диапазон от 64 до 1024");
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is StorageDevice))
                return false;

            var temp = obj as StorageDevice;

            if (temp.SpeedRead != this.SpeedRead) return false;
            if (temp.SpeedWrite != this.SpeedWrite) return false;
            if (temp.Size != this.Size) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
    