using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model.Procedures
{
    public class FixedTimeBlock:AbstractBlock
    {
        public double FixedTime { get; protected set; }

        public override string Description => "Блок фиксированного времени";

        public FixedTimeBlock(double fixedTime):base(1,1)
        {
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
    }
}
