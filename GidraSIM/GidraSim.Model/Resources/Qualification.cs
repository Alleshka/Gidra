using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSim.Model.Resources
{
    public class Qualification
    {
        private byte _efficiency;
        private byte _errorProbability;
        public virtual short QualificationId { get; set; }

        public virtual byte Efficiency  
        {
            get { return _efficiency; }
            set
            {   if(value>=25&&value<=200)
                    _efficiency = value;
                else
                {
                    throw new Exception($"Значение должно входить в диапазон от 25 до 200");
                }
            }
        }

        public virtual byte ErrorProbability    
        {
            get { return _errorProbability; }
            set
            {
                if(value>=0&&value<=25)
                    _errorProbability = value;
                else
                {
                    throw new Exception($"Значение должно входить в диапазон от 0 до 25");
                }
            }
        }
    }
}
