using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Core.Model.Procedures
{
    public abstract class AbstractProcedure : AbstractBlock, IProcedure
    {
        //protected ConnectionManager connectionManager = new ConnectionManager();

        public override string Description => "Procedure";

        protected List<IResource> resources = new List<IResource>();

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
    }
}
