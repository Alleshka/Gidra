using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model.Procedures
{
    public class QualityCheckProcedure: AbstractProcedure
    {

        public override string Description => "Проверка качества";

        public QualityCheckProcedure() : base(1, 2)
        {
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
                    collector.Collect(token);
                    token.ProcessEndTime = modelingTime.Now;
                    if(rand.Next(0,100) > 70)
                        outputs[0] = new Token(modelingTime.Now, token.Complexity) { Parent = this };
                    else
                        outputs[1] = new Token(modelingTime.Now, token.Complexity) { Parent = this };
                }
            }
        }
    }
}
