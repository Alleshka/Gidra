using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(IsReference = true)]
    public abstract class AbstractResource : ThePrice, IResource
    {
        [DataMember]
        public short ID { get; set; }

        [DataMember(EmitDefaultValue = false)]
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
            if (!(obj is AbstractResource))
                return false;

            AbstractResource res = obj as AbstractResource;

            if (res.Description != this.Description)
                return false;

            if (res.Price != this.Price) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public virtual bool TryUseResource(ModelingTime time)
        {
            return true;
        }
    }
}
