using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model
{
    class EndBlock:Block
    {
        public override string Description => "End block";

        public EndBlock(int inNumber, ITokensCollector collector):base(inNumber, 0, collector)
        {

        }
    }
}
