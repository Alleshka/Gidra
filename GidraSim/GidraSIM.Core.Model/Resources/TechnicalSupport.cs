using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace GidraSIM.Core.Model.Resources
{
    // TODO: Переписать повидение цены
    // TODO: Не уверен, что от абстрактного ресурса норм наследовать

    [DataContract(IsReference = true)]
    public class TechnicalSupport : AbstractResource
    {
        //public short TechnicalSupportId { get; set; }

        [DataMember]
        public CPU Cpu { get; set; }

        [DataMember]
        public RAM Ram { get; set; }

        [DataMember]
        public GPU Gpu { get; set; }

        [DataMember]
        public StorageDevice StorageDevice { get; set; }

        [DataMember]
        public Monitor Monitor { get; set; }

        [DataMember]
        public IEnumerable<InputDevices> InputDeviceses { get; set; }

        [DataMember]
        public IEnumerable<Printer> Printers { get; set; }

        //public IEnumerable<Procedure> Procedures { get; set; }  


        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is TechnicalSupport))
                return false;

            var temp = obj as TechnicalSupport;

            if (temp.InputDeviceses.Count() != this.InputDeviceses.Count()) return false;
            if (temp.Printers.Count() != this.Printers.Count()) return false;

            if (!Equals(temp.Cpu, this.Cpu)) return false;
            if (!Equals(temp.Ram, this.Ram)) return false;
            if (!Equals(temp.Gpu, this.Gpu)) return false;
            if (!Equals(temp.StorageDevice, this.StorageDevice)) return false;
            if (!Equals(temp.Monitor, this.Monitor)) return false;

            for (int i = 0; i < temp.InputDeviceses.Count(); i++)
            {
                if (!Equals(temp.InputDeviceses.Skip(i).First(), this.InputDeviceses.Skip(i).First())) return false;
            }

            for (int i = 0; i < temp.Printers.Count(); i++)
            {
                if (!Equals(temp.Printers.Skip(i).First(), this.Printers.Skip(i).First())) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
