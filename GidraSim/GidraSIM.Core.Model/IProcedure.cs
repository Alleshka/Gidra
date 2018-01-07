using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Core.Model
{
    public interface IProcedure:IBlock
    {
        void AddResorce(IResource resource);
        void ClearResources();
    }
}
