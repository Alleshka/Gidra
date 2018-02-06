using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model
{
    [DataContract(IsReference = true)]
    public abstract class AbstractBlock: ThePrice, IBlock
    {
        protected int outputQuantity;
        protected int inputQuantity;

        [DataMember(EmitDefaultValue = false)]
        public int OutputQuantity
        {
            get => outputQuantity;
            set
            {
                outputQuantity = value;
                OnOutputQuantityChanged();
            }
        }
        [DataMember(EmitDefaultValue = false)]
        public int InputQuantity
        {
            get => inputQuantity;
            set
            {
                inputQuantity = value;
                OnInputQuantityChanged();
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Description
        {
            get;
            set;
        }

        public virtual void OnOutputQuantityChanged()
        {
            outputs = new Token[OutputQuantity];
        }

        public virtual void OnInputQuantityChanged()
        {
            inputQueue = new Queue<Token>[InputQuantity];
            for (int i = 0; i < InputQuantity; i++)
            {
                inputQueue[i] = new Queue<Token>();
            }
        }

        public override string ToString()
        {
            return this.Description;
        }

        [DataMember(EmitDefaultValue = false)]
        protected Queue<Token>[] inputQueue;

        /*/// <summary>
        /// пары объект-блок
        /// </summary>
        protected Tuple<IBlock,int>[] outputs;*/
        [DataMember(EmitDefaultValue = false)]
        protected Token[] outputs;

        [DataMember(EmitDefaultValue = false)]
        protected ITokensCollector collector;
        public ITokensCollector TokenCollector { get => collector; }

        //protected double globalTime;

        public AbstractBlock(int inQuantiry, int outQuantity)
        {
            this.InputQuantity = inQuantiry;
            this.OutputQuantity = outQuantity;
            collector = TokensCollector.GetInstance();
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

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;

            if (!(obj is AbstractBlock)) return false;
            AbstractBlock temp = obj as AbstractBlock;

            if (temp.InputQuantity != this.InputQuantity)
                return false;
            if (temp.inputQueue.Length != this.inputQueue.Length)
                return false;
            if (temp.OutputQuantity != this.OutputQuantity)
                return false;
            if (temp.outputs.Length != this.outputs.Length)
                return false;
            if ((temp.TokenCollector != this.TokenCollector))
                return false;

            for (int i = 0; i < temp.inputQueue.Count(); i++)
            {
                if (temp.inputQueue[i].Count() != this.inputQueue[i].Count())
                    return false;

                //var tempArray1 = temp.inputQueue[i].ToArray();
                //var tempArray2 = this.inputQueue[i].ToArray();

                //for (int j = 0; j < tempArray1.Count(); j++)
                //{
                //    if (!Equals(tempArray1[j], tempArray2[j])/*&&(tempArray1[i]!=tempArray2[i])*/)
                //        return false;
                //}
            }

            //for (int i = 0; i < temp.outputs.Length; i++)
            //{
            //    if((!Equals(this.outputs[i], temp.outputs[i])))
            //        return false;
            //}

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void CleaInputs()
        {
            //for (int i = 0; i < InputQuantity; i++)
            //{
            //    inputQueue[i] = new Queue<Token>();
            //}           
            foreach (var q in inputQueue)
            {             
                q.Clear();
            }
        }
    }
}
