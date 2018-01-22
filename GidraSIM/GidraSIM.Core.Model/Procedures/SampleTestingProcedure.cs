using System;
using System.Linq;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Procedures
{
    [DataContract(IsReference = true)]
    public class SampleTestingProcedure:AbstractProcedure
    {

        public SampleTestingProcedure():base(1,1)
        {
            Description = "Тестирование образца";
        }

        public override void Update(ModelingTime modelingTime)
        {
            if (inputQueue[0].Count() > 0)
            {
                Random rand = new Random();
                var token = inputQueue[0].Peek();


                if (token.Progress < 0.0001)
                {
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = modelingTime.Now;

                }

                double time = token.Complexity * 100;

                token.Progress += modelingTime.Delta / time;

                if (token.Progress > 0.99)
                {
                    inputQueue[0].Dequeue();
                    token.ProcessEndTime = modelingTime.Now;
                    collector.Collect(token);

                    outputs[0] = new Token(modelingTime.Now, token.Complexity) { Parent = this };
                }
            }
        }
    }
}
