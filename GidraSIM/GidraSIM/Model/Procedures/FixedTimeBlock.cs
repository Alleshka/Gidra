using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model.Procedures
{
    public class FixedTimeBlock:Block
    {
        public double FixedTime { get; protected set; }
        public double prevTime = 0;

        public FixedTimeBlock(ITokensCollector collector, double fixedTime):base(1,1,collector)
        {
            FixedTime = fixedTime;
        }

        public override void Update(double globalTime)
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
                    token.ProcessStartTime = globalTime;
                } 

                //времени прошло больше чем фиксирвоаннное время
                if(globalTime - token.ProcessStartTime >= FixedTime)
                {
                    inputQueue[0].Dequeue();
                    collector.Collect(token);

                    outputs[0] = new Token(globalTime, token.Complexity) { Parent = this };
                }
            }
            prevTime = globalTime;
        }
    }
}
