using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Core.Model.Resources
{
    public abstract class Resource : IResource
    {
        public string Description { get; set; }

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
