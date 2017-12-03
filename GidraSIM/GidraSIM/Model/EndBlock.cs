using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model
{
    public class EndBlock:Block
    {
        public override string Description => "End block";

        public EndBlock(int inNumber, ITokensCollector collector):base(inNumber, 0, collector)
        {

        }

        public override void Update(double globalTime)
        {
            //просто тупая очистка всех входных очередей
            foreach (var q in inputQueue)
            {
                if (q.Count != 0)
                {
                    var token = q.Dequeue();
                    token.Progress = 1.0;
                    token.Description = this.Description;
                    token.ProcessedByBlock = this;
                    //сброс в мусорку
                    collector.Collect(token);
                }
            }
        }
    }
}
