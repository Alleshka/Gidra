using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSim.Model.Resources
{
    public enum TypeIS
    {
        Бумажный,
        Электронный
    }

    public class InformationSupport
    {
        public virtual short InformationSupportId { get; set; }

        public virtual bool MultiClientUse { get; set; }

        public virtual TypeIS Type { get; set; }
    }
}
