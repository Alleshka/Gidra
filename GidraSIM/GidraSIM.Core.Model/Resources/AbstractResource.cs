using System;
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

        [DataMember(EmitDefaultValue = false)]
        public IAccidentsCollector Collector;

        [DataMember(EmitDefaultValue = false)]
        public int Count { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int MaxCount { get; set; }

        public AbstractResource()
        {
            Collector = AccidentsCollector.GetInstance();
            Count = 1;
            MaxCount = 1;
        }

        public virtual void ReleaseResource()
        {
            //do nothing
            if (Count + 1 <= MaxCount)
                Count += 1;
            else
                throw new IndexOutOfRangeException("Попытка вернуть большее число ресурса " + this.ToString() + " чем возможно");
        }

        public virtual bool TryGetResource()
        {
            if (Count != 0)
            {
                Count -= 1;
                return true;
            }
            else
                return false;
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
