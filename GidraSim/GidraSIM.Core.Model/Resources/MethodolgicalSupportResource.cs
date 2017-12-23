using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model.Resources
{
    public class MethodolgicalSupportResource:Resource
    {
        /// <summary>
        /// методологические ресурсы всегда доступны
        /// </summary>
        /// <returns></returns>
        public override bool TryGetResource() => true;

        public MethodolgicalSupportResource()
        {
            Description = "Гугл";
        }
    }
}
