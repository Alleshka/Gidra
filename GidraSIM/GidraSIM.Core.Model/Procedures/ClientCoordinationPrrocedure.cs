using System;
using System.Linq;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Procedures
{
    [DataContract(IsReference = true)]
    public class ClientCoordinationPrrocedure:AbstractProcedure
    {

        public ClientCoordinationPrrocedure():base(1,1)
        {
            Description = "Согласование доков с заказчиком";
        }

        public override void Update(ModelingTime modelingTime)
        {
            if (inputQueue[0].Count() > 0)
            {
                Random rand = new Random();
                var token = inputQueue[0].Peek();


                if (token.Progress < 0.01)
                {
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = modelingTime.Now;

                }

                double time = rand.Next(1, 30);//случайное число, т.к. сроки размыты // порядок дни/месяцы
                //базовое время зависит от сроков сдачи проекта разрабатываемого объекта, которое в данной работе не учитывается

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
