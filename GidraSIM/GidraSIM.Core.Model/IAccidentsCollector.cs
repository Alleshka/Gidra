using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model
{
    public interface IAccidentsCollector
    {
        List<Accident> GetHistory();
        void Collect(Accident accident);
    }
}
