using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model
{
    public class StartBlock:Block
    {

        public override string Description => "Start block";

        //тестовый блок
        public StartBlock(int outNumber, ITokensCollector collector):base(0, outNumber, collector)
        {
            for(int i=0; i< outNumber; i++)
            {
                var token =
                outputs[i] = new Token(0, 1.0) { Parent = this };
            }
        }

        public override void Update(double dt)
        {
            //base.Update(dt);
        }
    }
}
