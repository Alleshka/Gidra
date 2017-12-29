using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model
{
    public interface IResource
    {
        bool TryGetResource();
        void ReleaseResource();
        string Description { get; set; }
    }
}
