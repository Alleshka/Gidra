using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [Serializable]
    public enum Qualification
    {
        [Description("Без опыта")]
        NoCategory,
        [Description("Новичок")]
        FirstCategory,
        [Description("Среднячок")]
        SecondCategory,
        [Description("Опытный")]
        ThirdCategory,
        [Description("Гуру")]
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

        private double accidentEndTime = 0;
        private const int accidentProbability = 20;

        public override bool TryUseResource(ModelingTime time)
        {
            //время инцидента  вышло
            if (accidentEndTime <= time.Now)
            {
                Random random = new Random();
                if (random.Next(0, 100) < accidentProbability)
                {
                    accidentEndTime = time.Now + random.Next(5, 12);
                    return false;
                }
                return true;
            }
            //время инцидента  не вышло
            else
            {
                return false;
            }
            //return base.TryUseResource(time);
        }
    }
}
