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
    public class WorkerResource : AbstractResource
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
            if (!base.Equals(obj))
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

        [DataMember(EmitDefaultValue = false)]
        private Accident accident = null;
        private double accidentProbability = 20;

        [DataMember(EmitDefaultValue = false)]
        public double AccidentProbability { get => accidentProbability; set { accidentProbability = value; } }

        public override bool TryUseResource(ModelingTime time)
        {
            //нет текущего инцидента
            if (accident == null)
            {
                //пробуем создать новый инцидент
                Random random = new Random();
                double r = random.NextDouble() * time.Delta*100.0;
                if (r < accidentProbability)
                {
                    accident = new Accident() { Source = this, Description = "Болезнь работника" };

                    accident.StartTime = time.Now;
                    //от 5 до 11 дней болезни
                    accident.EndTime = time.Now + random.Next(5, 12);
                    //всё, заболел
                    return true;
                }
                //не удалось, всё ок
                return true;
            }
            //есть инцидент
            else
            {
                //время инцидента  вышло      
                if (accident.EndTime <= time.Now)
                {
                    Collector.Collect(accident);
                    accident = null;
                    //всё ок
                    return true;
                }
                //время инцидента  не вышло
                else
                {
                    //всё ещё болеет
                    return false;
                }
            }
            //return base.TryUseResource(time);
        }
    }
}
