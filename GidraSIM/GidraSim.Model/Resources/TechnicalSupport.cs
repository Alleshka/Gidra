using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GidraSim.Model.Processes;

namespace GidraSim.Model.Resources
{
    public class TechnicalSupport
    {
        public short TechnicalSupportId { get; set; }

        public Cpu Cpu { get; set; }

        public Ram Ram { get; set; }

        public Gpu Gpu { get; set; }

        public StorageDevice StorageDevice { get; set; }

        public Monitor Monitor { get; set; }

        public IEnumerable<InputDevices> InputDeviceses { get; set; }

        public IEnumerable<Printer> Printers { get; set; }

        public IEnumerable<Procedure> Procedures { get; set; }  
    }
}
