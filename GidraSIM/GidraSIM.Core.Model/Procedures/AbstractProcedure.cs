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
        public override string Description => "Procedure";

        [DataMember]
        protected List<IResource> resources = new List<IResource>();
        public int ResourceCount { get => resources.Count; }

        public AbstractProcedure(int inQuantity, int outQuantity) : base(inQuantity, outQuantity)
        {

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
    }
}
