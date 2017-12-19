using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model
{
    public class Block: IBlock
    {
        public int OutputQuantity { get; protected set; }
        public int InputQuantity { get; protected set; }

        public virtual string Description
        {
            get
            {
                return "Block";
            }
        }

        public override string ToString()
        {
            return this.Description;
        }

        protected Queue<Token>[] inputQueue;

        /*/// <summary>
        /// пары объект-блок
        /// </summary>
        protected Tuple<IBlock,int>[] outputs;*/
        protected Token[] outputs;
        protected ITokensCollector collector;

        //protected double globalTime;


        public Block(int inQuantiry, int outQuantity, ITokensCollector collector)
        {
            this.InputQuantity = inQuantiry;
            this.OutputQuantity = outQuantity;
            inputQueue = new Queue<Token>[InputQuantity];
            for(int i=0; i<InputQuantity; i++)
            {
                inputQueue[i] = new Queue<Token>();
            }

            outputs = new Token[OutputQuantity];
            this.collector = collector;
        }

        public virtual void AddToken(Token token, int inputNumber)
        {
            inputQueue[inputNumber].Enqueue(token);
        }


        public virtual void Update(ModelingTime modelingTime)
        {
            bool wasTokens = false;
            //просто тупая очистка всех входных очередей
            foreach(var q in inputQueue)
            {
                if (q.Count != 0)
                {
                    var token = q.Dequeue();
                    token.Progress = 1.0;
                    token.Description = this.Description;
                    //сброс в мусорку
                    collector.Collect(token);
                    wasTokens = true;
                }
            }
            //выдать, если хоть что-то было на входе
            //просто для теста, в еральном блоке иначе
            if (wasTokens && false)
            {
                for (int i = 0; i < OutputQuantity; i++)
                {
                    outputs[i] = new Token(modelingTime.Now, 1.0);
                }
            }
        }

        public void ClearOutputs()
        {
            for (int i = 0; i < OutputQuantity; i++)
            {
                outputs[i] = null;
            }
        }

        public virtual Token GetOutputToken(int port)
        {
            return outputs[port];
        }
    }
}
