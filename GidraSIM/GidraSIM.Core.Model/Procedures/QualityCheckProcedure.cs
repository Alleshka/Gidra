using System;
using System.Linq;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Procedures
{
    [DataContract(IsReference = true)]
    public class QualityCheckProcedure: AbstractProcedure
    {
        [DataMember(EmitDefaultValue = false)]
        /// <summary>
        /// вероятность успешщно пройти проверку
        /// </summary>
        public int Probability
        {
            get;
            set;
        }

        public QualityCheckProcedure() : base(1, 2)
        {
            Probability = 70;
            Description = "Проверка качества";
        }

        public override void Update(ModelingTime modelingTime)
        {
            if (inputQueue[0].Count() > 0)
            {
                Random rand = new Random();
                var token = inputQueue[0].Peek();


                if (token.Progress == 0)
                {
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = modelingTime.Now;

                }

                double time = token.Complexity * 1;

                token.Progress += modelingTime.Delta / time;

                if (token.Progress > 0.99)
                {
                    inputQueue[0].Dequeue();
                    token.ProcessEndTime = modelingTime.Now;
                    collector.Collect(token);
                    if(rand.Next(0,100) < Probability)
                        outputs[0] = new Token(modelingTime.Now, token.Complexity) { Parent = this };
                    else
                        outputs[1] = new Token(modelingTime.Now, token.Complexity) { Parent = this };
                }
            }
        }
    }
}
