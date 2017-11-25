using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Model
{
    public class Procedure : Block, IProcedure
    {
        protected ConnectionManager connectionManager = ConnectionManager.GetInstance();

        public override string Description => "Procedure";

        List<IResource> resources = new List<IResource>();

        public Procedure(int inQuantity, int outQuantity, ITokensCollector collector) : base(inQuantity, outQuantity, collector)
        {
        }

        public void AddResorce(IResource resource)
        {
            resources.Add(resource);
        }

        public override void Update(double dt)
        {
            
            foreach(var resource in resources)
            {
                //если ресурс недоступен, то ничего не делать
                if (resource.TryGetResource() == false)
                    return;
            }
            base.Update(dt);
        }
    }
}
