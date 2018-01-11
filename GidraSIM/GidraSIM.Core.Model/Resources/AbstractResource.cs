using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(IsReference = true)]
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

        public override bool Equals(object obj)
        {
            if (!(obj is AbstractResource)) return false;
            AbstractResource res = obj as AbstractResource;
            if (res.Description != this.Description)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
