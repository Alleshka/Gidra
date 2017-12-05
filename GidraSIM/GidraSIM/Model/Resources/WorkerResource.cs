using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model.Resources
{
    
    public class WorkerResource: Resource
    {
        public enum Qualification
        {
            NoCategory,
            FirstCategory,
            SecondCategory,
            ThirdCategory,
            LeadCategory//Ведущий инженеар, например
        };
        public Qualification WorkerQualification
        {
            get;
            set;
        }

        /// <summary>
        /// ФИО
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// должность
        /// </summary>
        public string Position
        {
            get;
            set;
        }
    }
}
