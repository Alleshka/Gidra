using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraCore
{
    class TestRootProcess:Process
    {
        public TestRootProcess():
            base(0,0)
        {
            StartBlock startBlock = new StartBlock(1);
            EndBlock endBlock = new EndBlock(1);
            this.allBlocks.Add(startBlock);
            this.allBlocks.Add(endBlock);
            var block1 = new Block(1, 2);
            allBlocks.Add(block1);

            var block2_1 = new Block(1, 1);
            allBlocks.Add(block2_1);

            var block2_2 = new Block(1, 1);
            allBlocks.Add(block2_2);

            var block3 = new Block(3, 2);
            allBlocks.Add(block3);

            startBlock.Connect(0, block1, 0);

            block1.Connect(0, block2_1, 0);
            block1.Connect(1, block2_2, 0);

            block2_1.Connect(0, block3, 0);
            block2_2.Connect(0, block3, 1);

            block3.Connect(0, endBlock, 0);
            //обратная связь
            block3.Connect(1, block3, 2);
        }
    }
}
