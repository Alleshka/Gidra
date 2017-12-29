using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(Name = "MethodolgicalSupportResource")]
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
