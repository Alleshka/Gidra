using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model.Procedures
{
    public class DocumentationCoordinationProcedure: Procedure
    {
        public DocumentationCoordinationProcedure(ITokensCollector collector):base(1,1,collector)
        {

        }

        public override void Update(ModelingTime modelingTime)
        {
            if (inputQueue.Count() > 0)
            {
                Random rand = new Random();
                var token = inputQueue[0].Peek();


                if (token.Progress < 0.01)
                {
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = modelingTime.Now;
                   
                }

                double time = rand.Next(7, 60);//случайное число, т.к. сроки размыты // порядок дни/месяцы
                //базовое время зависит от сроков сдачи проекта разрабатываемого объекта, которое в данной работе не учитывается

                token.Progress += modelingTime.Delta / time;

                if (token.Progress > 0.99)
                {
                    inputQueue[0].Dequeue();
                    collector.Collect(token);

                    outputs[0] = new Token(modelingTime.Now, token.Complexity) { Parent = this };
                }
            }
        }
    }
}
