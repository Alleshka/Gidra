using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Model
{
    public abstract class Resource : IResource
    {
        public void ReleaseResource()
        {
            //do nothing
        }

        public bool TryGetResource()
        {
            return true;
        }
    }
}
