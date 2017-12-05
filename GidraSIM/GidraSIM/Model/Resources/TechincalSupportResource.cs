using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model.Resources
{
    public class TechincalSupportResource: Resource
    {
        public TechincalSupportResource()
        {
            Count = 1;
        }

        public double Frequency
        {
            get;
            set;
        }

        public double Ram
        {
            get;
            set;
        }
        public double Vram
        {
            get;
            set;
        }

        /// <summary>
        /// число экземпляров
        /// </summary>
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
