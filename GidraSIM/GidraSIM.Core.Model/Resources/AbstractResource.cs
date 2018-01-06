using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract]
    public abstract class AbstractResource : IResource
    {
        [DataMember]
        public string Description { get; set; }

        public virtual void ReleaseResource()
        {
            //do nothing
        }

        public virtual bool TryGetResource()
        {
            return true;
        }

        public override string ToString() => Description;
    }
}
