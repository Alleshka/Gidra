using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model
{
    /// <summary>
    /// блок, имеющию внутри другие блоки
    /// </summary>
    [DataContract(IsReference =true)]
    public class Process: AbstractBlock
    {        
        [DataMember(EmitDefaultValue = false)]
        public IConnectionManager Connections
        {
            get;
            /*protected*/ set;
        }

        public ITokensCollector Collector
        {
            get => collector;
        }

        [DataMember(EmitDefaultValue = false)]
        public IBlock StartBlock
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public IBlock EndBlock
        {
            get;
            set;
        }

        public Process():base(1,1)
        {
            Blocks = new List<IBlock>();
            Connections = new ConnectionManager();
            Resources = new List<IResource>();
        }

        [DataMember(EmitDefaultValue = false)]
        public List<IBlock> Blocks
        {
            get;
            protected set;
        }

        [DataMember(EmitDefaultValue = false)]
        public List<IResource> Resources
        {
            get;
            protected set;
        }

        /// <summary>
        /// индикатор токена на 0 выходе последнего блока
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool EndBlockHasOutputToken
        {
            get;
            /*private*/ set;
        }

        /// <summary>
        /// осуществляет обработку и пееремещеник блоков внутри себя
        /// </summary>
        /// <param name="globalTime"></param>
        public override void Update(ModelingTime modelingTime)
        {
            //костыль для работы блоков параллельности и и
            if (modelingTime.Now != 0)
            {
                //апдейт блоков пост-фактум
                for (int i = 0; i < Blocks.Count; i++)
                {
                    Blocks[i].ClearOutputs();
                }
            }


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
        }

        /// <summary>
        /// очистить всё содержимое процесса
        /// </summary>
        public void ClearProcess()
        {
            //по идее на обычные блоки нельзя ссылаться из других процессов, поэтому чистим и соединения ресурсов
            foreach(var block in Blocks)
            {
                if (block is IProcedure)
                    (block as IProcedure).ClearResources();
                block.CleaInputs();
                block.ClearOutputs();
            }
            Connections.GetAllConnections().Clear();
            StartBlock = null;
            EndBlock = null;
            EndBlockHasOutputToken = false;
            Blocks.Clear();
            Resources.Clear();
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


        public override bool Equals(object obj)
        {
            if(!base.Equals(obj))
                return false;

            Process temp = obj as Process;

            if (temp.Blocks.Count != this.Blocks.Count)
                return false;
            if (temp.EndBlockHasOutputToken != this.EndBlockHasOutputToken)
                return false;
            if (temp.Resources.Count != this.Resources.Count)
                return false;

            if (/*(temp.Connections != this.Connections)&&*/(!Equals(temp.Connections, this.Connections)))
                return false;
            if (/*(temp.EndBlock != this.EndBlock) && */(!Equals(temp.EndBlock, this.EndBlock)))
                return false;
            if (/*(temp.StartBlock != this.StartBlock) && */(!Equals(temp.StartBlock, this.StartBlock)))
                return false;

            for (int i = 0; i < temp.Resources.Count; i++)
            {
                if (/*temp.Resources[i] != this.Resources[i] && */!Equals(temp.Resources[i], this.Resources[i]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
