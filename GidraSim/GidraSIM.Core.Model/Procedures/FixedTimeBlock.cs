using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Procedures
{
    [DataContract(IsReference = true)]
    public class FixedTimeBlock:AbstractBlock
    {
        [DataMember(EmitDefaultValue = false)]
        public double FixedTime { get; set; }

        public FixedTimeBlock(double fixedTime):base(1,1)
        {
            Description = "Блок фиксированного времени";
            FixedTime = fixedTime;
        }

        public override void Update(ModelingTime modelingTime)
        {
            if (inputQueue[0].Count > 0)
            {
                //смотрим на токен
                var token = inputQueue[0].Peek();
                //токен в первый раз?
                if( token.Progress  < 0.1 )
                {
                    token.Progress = 1;
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = modelingTime.Now;
                } 

                //времени прошло больше чем фиксирвоаннное время
                if(modelingTime.Now - token.ProcessStartTime >= FixedTime)
                {
                    inputQueue[0].Dequeue();
                    token.ProcessEndTime = modelingTime.Now;
                    collector.Collect(token);

                    outputs[0] = new Token(modelingTime.Now, token.Complexity) { Parent = this };
                }
            }
        }

        public override bool Equals(object obj)
        {
            if(!base.Equals(obj)) return false;

            FixedTimeBlock temp = obj as FixedTimeBlock;
            if (temp.FixedTime != this.FixedTime) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
