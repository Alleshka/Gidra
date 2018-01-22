using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(IsReference = true)]
    public class MethodolgicalSupportResource:AbstractResource
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
