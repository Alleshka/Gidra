using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model
{
    public class TestTpprProcedure:Process
    {
        List<IBlock> allBlocks = new List<IBlock>();

        public TestTpprProcedure(ITokensCollector tokensCollector):
            base(tokensCollector)
        {
            StartBlock startBlock = new StartBlock(1, collector);
            EndBlock endBlock = new EndBlock(1, collector);
            this.allBlocks.Add(startBlock);
            this.allBlocks.Add(endBlock);
            var block1 = new Block(1, 2, collector);
            allBlocks.Add(block1);

            var block2_1 = new Block(1, 1, collector);
            allBlocks.Add(block2_1);

            var block2_2 = new Block(1, 1, collector);
            allBlocks.Add(block2_2);

            var block3 = new Block(3, 2, collector);
            allBlocks.Add(block3);

            this.Connections.Connect(startBlock, 0, block1, 0);
            //параллельные блоки
            this.Connections.Connect(block1, 0, block2_1, 0);
            this.Connections.Connect(block1, 1, block2_2, 0);
            this.Connections.Connect(block2_1, 0, block3, 0);
            this.Connections.Connect(block2_2, 0, block3, 1);
            this.Connections.Connect(block3, 0, endBlock, 0);

            //обратная связь
            this.Connections.Connect(block3, 1, block3, 2);

            //переместить начальные токены
            //connectionManager.MoveTokens();
        }

        public override void Update(double globalTime)
        {
            //не нужно
            //base.Update(dt);

            //апдейт блоков
            for(int i=0; i<allBlocks.Count;i++)
            {
                allBlocks[i].Update(globalTime);
            }
            //перемещение токенов
            Connections.MoveTokens();

            //апдейт блоков
            for (int i = 0; i < allBlocks.Count; i++)
            {
                allBlocks[i].ClearOutputs();
            }
        }
    }
}
