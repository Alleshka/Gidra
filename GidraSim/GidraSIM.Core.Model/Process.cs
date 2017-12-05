using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model
{
    /// <summary>
    /// блок, имеющию внутри другие блоки
    /// </summary>
    public class Process:Block
    {
        public IConnectionManager Connections
        {
            get;
            protected set;
        }
        public ITokensCollector Collector
        {
            get => collector;
        }

        public IBlock StartBlock
        {
            get;
            set;
        }

        public IBlock EndBlock
        {
            get;
            set;
        }

        public Process(ITokensCollector collector):base(1,1, collector)
        {
            Blocks = new List<IBlock>();
            Connections = new ConnectionManager();
            Resources = new List<IResource>();
        }

        public List<IBlock> Blocks
        {
            get;
            protected set;
        }

        public List<IResource> Resources
        {
            get;
            protected set;
        }

        /// <summary>
        /// индикатор токена на 0 выходе последнего блока
        /// </summary>
        public bool EndBlockHasOutputToken
        {
            get;
            private set;
        }

        /// <summary>
        /// осуществляет обработку и пееремещеник блоков внутри себя
        /// </summary>
        /// <param name="globalTime"></param>
        public override void Update(ModelingTime modelingTime)
        {
            EndBlockHasOutputToken = false;
            //апдейт блоков
            for (int i = 0; i < Blocks.Count; i++)
            {
                Blocks[i].Update(modelingTime);
            }
            //перемещение токенов
            Connections.MoveTokens();

            if(EndBlock.GetOutputToken(0) != null)
            {
                EndBlockHasOutputToken = true;
            }

            //апдейт блоков
            for (int i = 0; i < Blocks.Count; i++)
            {
                Blocks[i].ClearOutputs();
            }
        }

        public override void AddToken(Token token, int inputNumber)
        {
            if (inputNumber != 0)
                throw new ArgumentOutOfRangeException("Процесс содержит только один вход!");
            StartBlock.AddToken(token, 0);
        }

        public override Token GetOutputToken(int port)
        {
            if (port != 0)
                throw new ArgumentOutOfRangeException("Процесс содержит только один выход!");
            return EndBlock.GetOutputToken(0);
        }
    }
}
