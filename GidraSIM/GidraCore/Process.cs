using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraCore
{
    class Process:Block
    {
        

        protected Queue<IBlock> unprocessedQueue = new Queue<IBlock>();
        protected List<IBlock> allBlocks = new List<IBlock>();

        public Process(int inNumber, int outNumber):
            base(inNumber,outNumber)
        {
        }

        public override void Update(double dt)
        {
            while(unprocessedQueue.Count != 0)
            {
                var block = unprocessedQueue.Dequeue();
                //на всех входах есть токены
                if(block.AllInputesFilled())
                {
                    //выполнить симуляцию
                    block.Update(dt);
                }
                //ещё не все входы заполнились
                else
                {
                    //отправить дальше ожидать
                    unprocessedQueue.Enqueue(block);
                }
            }
        }
    }
}
