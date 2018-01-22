using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [Serializable]
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


    [DataContract(IsReference = true)]
    public class WorkerResource: AbstractResource
    {
        public WorkerResource()
        {
            //Name = "Михалыч";
            //Position = "Работяга";
            Description = "Простой работник";
        }

        [DataMember(EmitDefaultValue = false)]
        public Qualification WorkerQualification
        {
            get;
            set;
        }

        ///// <summary>
        ///// ФИО
        ///// </summary>
        //public string Name
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// должность
        ///// </summary>
        //public string Position
        //{
        //    get;
        //    set;
        //}

        public override bool Equals(object obj)
        {
            if(!base.Equals(obj))
                return false;

            if (!(obj is WorkerResource))
                return false;
            WorkerResource temp = obj as WorkerResource;
            if (temp.WorkerQualification != this.WorkerQualification)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
