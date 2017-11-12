using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Model
{
    interface IProcedure:IBlock
    {
        void Connect(Resource resource);
    }
}
