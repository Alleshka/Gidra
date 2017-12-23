using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model.Resources
{
    
    public enum Qualification
    {
        [Description("Без категории")]
        NoCategory,
        [Description("Первая категория")]
        FirstCategory,
        [Description("Вторая категория")]
        SecondCategory,
        [Description("Третья категория")]
        ThirdCategory,
        [Description("Ведущий специалист")]
        LeadCategory//Ведущий инженеар, например
    };


    public class WorkerResource: Resource
    {

        public WorkerResource()
        {
            Name = "Михалыч";
            Position = "Работяга";
            Description = "Простой работник";
        }


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
