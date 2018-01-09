using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(Name = "TechincalSupportResource")]
    public class TechincalSupportResource: AbstractResource
    {
        public TechincalSupportResource()
        {
            Count = 1;
            Description = "Компьютер";
        }

        [DataMember]
        public double Frequency
        {
            get;
            set;
        }

        [DataMember]
        public double Ram
        {
            get;
            set;
        }
        [DataMember]
        public double Vram
        {
            get;
            set;
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
            if(Count > 0)
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

    }
}
