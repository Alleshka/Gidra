using GidraSIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            TokensCollector collector = new TokensCollector();
            ConnectionManager connectionManager = ConnectionManager.GetInstance();
            /*TestTpprProcedure testTpprProcedure = new TestTpprProcedure(tokensCollector);

            int prevCount = 0;
            int currentCount = -1;
            while (prevCount != currentCount && currentCount <100)
            {
                testTpprProcedure.Update(1);

                prevCount = currentCount;
                currentCount = tokensCollector.GetHistory().Count;
            }
            Console.WriteLine("Test passed");*/

            List<IBlock> allBlocks = new List<IBlock>();

            StartBlock startBlock = new StartBlock(1, collector);
            EndBlock endBlock = new EndBlock(1, collector);
            allBlocks.Add(startBlock);
            
            var block = new FixedTimeBlock(collector, 10);
            allBlocks.Add(block);
            allBlocks.Add(endBlock);

            connectionManager.Connect(startBlock, 0, block, 0);
            connectionManager.Connect(block, 0, endBlock, 0);


            int prevCount = 0;
            int currentCount = -1;
            do
            {
                for (int i = 0; i < allBlocks.Count; i++)
                {
                    allBlocks[i].Update(1);
                }
                connectionManager.MoveTokens();

                for (int i = 0; i < allBlocks.Count; i++)
                {
                    allBlocks[i].ClearOutputs();
                }

                collector.GlobalTime += 1;
                prevCount = currentCount;
                currentCount = collector.GetHistory().Count;
                if (currentCount > 0)
                {
                    if (collector.GetHistory().Last().ProcessedByBlock == endBlock)
                    {
                        break;
                    }
                }
            }
            while (true); 
            Console.WriteLine("Test passed");
        }
    }
}
