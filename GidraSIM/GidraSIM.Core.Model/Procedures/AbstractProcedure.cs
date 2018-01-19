using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using GidraSIM.Core.Model.Resources;

namespace GidraSIM.Core.Model.Procedures
{
    [DataContract(IsReference = true)]
    public abstract class AbstractProcedure : AbstractBlock, IProcedure
    {
        //protected ConnectionManager connectionManager = new ConnectionManager();

        [DataMember]
        protected List<IResource> resources = new List<IResource>();
        public int ResourceCount { get => resources.Count; }

        public AbstractProcedure(int inQuantity, int outQuantity) : base(inQuantity, outQuantity)
        {
            Description = "Procedure";
        }

        public void AddResorce(IResource resource)
        {
            resources.Add(resource);
        }

        public override void Update(ModelingTime modelingTime)
        {
            
            foreach(var resource in resources)
            {
                //если ресурс недоступен, то ничего не делать
                if (resource.TryGetResource() == false)
                    return;
            }
            base.Update(modelingTime);
        }

        public void ClearResources()
        {
            resources.Clear();
        }

        public override bool Equals(object obj)
        {
            if(!base.Equals(obj))
                return false;

            if (!(obj is AbstractProcedure)) return false;
            AbstractProcedure temp = obj as AbstractProcedure;

            if (temp.Description != this.Description)
                return false;
            if (temp.ResourceCount != this.ResourceCount)
                return false;


            for (int i = 0; i < temp.ResourceCount; i++)
            {
                if ((!Equals(temp.resources[i], this.resources[i])))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
