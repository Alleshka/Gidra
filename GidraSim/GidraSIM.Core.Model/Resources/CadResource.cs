using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Description = "CAD";
        }

        /// <summary>
        /// число экземпляров
        /// </summary>

        [DataMember]
        public int Count
        {
            get;
            set;
        }


        public override bool TryGetResource()
        {
            if (Count > 0)
            {
                Count--;
                return true;
            }
            return false;
        }

        public override void ReleaseResource()
        {
            //TODO чисто теоретически можно верунть больше чем есть, нужна защита от дурака
            Count++;
        }

        public override bool Equals(object obj)
        {
            if(!base.Equals(obj))
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
