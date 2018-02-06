using System.Collections.Generic;
using GidraSim.Model.Processes;

namespace GidraSim.Model.Resources
{
    public enum TypeIS
    {
        Бумажный,
        Электронный
    }

    public class InformationSupport:ThePrice
    {
        public virtual short InformationSupportId { get; set; }

        public virtual bool MultiClientUse { get; set; }

        public virtual TypeIS Type { get; set; }

        public IEnumerable<Process> Processes { get; set; } 
    }
}
