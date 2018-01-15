using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(IsReference = true)]
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

        public override bool Equals(object obj)
        {

            if(!base.Equals(obj))
                return false;

            if (!(obj is TechincalSupportResource))
                return false;
            TechincalSupportResource temp = obj as TechincalSupportResource;

            if (temp.Frequency != this.Frequency)
                return false;
            if (temp.Ram != this.Ram)
                return false;
            if (temp.Vram != this.Vram)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
