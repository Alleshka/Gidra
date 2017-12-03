using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model
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

        public override void Update(double globalTime)
        {

            //апдейт блоков
            for (int i = 0; i < Blocks.Count; i++)
            {
                Blocks[i].Update(globalTime);
            }
            //перемещение токенов
            Connections.MoveTokens();

            //апдейт блоков
            for (int i = 0; i < Blocks.Count; i++)
            {
                Blocks[i].ClearOutputs();
            }
        }
    }
}
