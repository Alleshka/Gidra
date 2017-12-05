using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Model.Resources
{
    public abstract class Resource : IResource
    {
        public virtual void ReleaseResource()
        {
            //do nothing
        }

        public virtual bool TryGetResource()
        {
            return true;
        }
    }
}
