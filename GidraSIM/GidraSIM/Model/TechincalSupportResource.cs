using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model
{
    public class TechincalSupportResource: Resource
    {

        public double Frequency
        {
            get => 1.5;
            set
            {
                Frequency = value;
            }
        }

        public double Ram
        {
            get => 2;
            set
            {
                Ram = value;
            }
        }
        public double Vram
        {
            get => 1;
            set
            {
                Vram = value;
            }
        }
    }
}
