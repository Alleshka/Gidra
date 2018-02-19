using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(IsReference = true)]
    public class CadResource:AbstractResource
    {
        //TODO добавить типы лицензии

        //TODO добавить правила лицензирования

        public CadResource()
        {
            Count = 10;
            MaxCount = 10;
            Description = "CAD";
        }


        public override bool TryGetResource()
        {
            return base.TryGetResource();
        }

        public override void ReleaseResource()
        {
             base.ReleaseResource();
        }

        public override bool Equals(object obj)
        {

            if(!base.Equals(obj))
                return false;

            if (!(obj is CadResource))
                return false;

            CadResource temp = obj as CadResource;
            if (temp.Count != this.Count)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
