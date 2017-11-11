using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraCore
{
    class Block: IBlock
    {
        public int OutputQuantity { get; protected set; }
        public int InputQuantity { get; protected set; }

        protected Queue<Token>[] inputQueue;

        /// <summary>
        /// пары объект-блок
        /// </summary>
        protected Tuple<IBlock,int>[] outputs;

        /// <summary>
        /// нужно ли проверить на всех входах ли есть токен
        /// </summary>
        protected bool needToCheckInputs = true;

        /// <summary>
        /// на всеъ входах есть токен
        /// </summary>
        protected bool allInputsFilled = false;

        public Block(int inNumber, int outNumber)
        {
            this.InputQuantity = inNumber;
            this.OutputQuantity = outNumber;
            inputQueue = new Queue<Token>[InputQuantity];
            outputs = new Tuple<IBlock, int>[OutputQuantity];
        }

        public void AddToken(Token token, int inputNumber)
        {
            inputQueue[inputNumber].Enqueue(token);
            needToCheckInputs = true;
        }

        public bool AllInputesFilled()
        {
            if (needToCheckInputs)
            {
                int count = 0;
                for (int i = 0; i< this.InputQuantity; i++)
                {
                    if (inputQueue[i].Count > 0)
                        count++;
                }
                if (count == InputQuantity)
                    allInputsFilled = true;
                else
                    allInputsFilled = false;
            }
            //только что посчитали, ничего считать не надо пока новые не придут
            needToCheckInputs = false;

            return allInputsFilled;
        }

        public void Connect(int outputNumber, IBlock block, int blockInputNumber)
        {
            outputs[outputNumber] = new Tuple<IBlock, int>(block, blockInputNumber);
        }

        /// <summary>
        /// отправить токен с указанного выхода
        /// </summary>
        /// <param name="token"></param>
        /// <param name="outputNumber"></param>
        void SendTokenFromOutput(Token token, int outputNumber)
        {
            var ep = outputs[outputNumber];
            ep.Item1.AddToken(token, ep.Item2);
        }

        public virtual void Update(double dt)
        {
            //просто тупая очистка всех входных очередей
            foreach(var q in inputQueue)
            {
                q.Clear();
            }

            for( int i=0;  i< OutputQuantity; i++)
            {
                SendTokenFromOutput(new Token(), i);
            }
        }
    }
}
