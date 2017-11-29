using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Model
{
    public class Resource : IResource
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
