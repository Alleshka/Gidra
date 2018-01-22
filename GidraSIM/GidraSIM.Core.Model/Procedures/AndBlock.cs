using System;
using System.Linq;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Procedures
{
    [DataContract(IsReference = true)]
    public class AndBlock : AbstractBlock
    {

        public AndBlock(int inputsQuantity) : base(inputsQuantity, 1)
        {
            Description = "&";
        }

        /// <summary>
        /// добавляет токен в очередь на обработку,
        /// если все входы заполнены, в тот же момент добавляет токен на выход
        /// </summary>
        /// <param name="token"></param>
        /// <param name="inputNumber"></param>
        public override void AddToken(Token token, int inputNumber)
        {
            base.AddToken(token, inputNumber);

            for (int i = 0; i < this.InputQuantity; i++)
            {
                //если на всех блоках чего-нибудь нет, ничего не делаем
                if (inputQueue[i].Count() == 0)
                    return;
            }
            double complexity = 0;
            double lastBornTime = 0;
            //ищем среднюю сложность и время рождения последнего из них
            //время рождения последнего из них - текущее время
            for (int i = 0; i < this.InputQuantity; i++)
            {
                var tempToken = inputQueue[i].Peek();
                complexity += tempToken.Complexity;
                lastBornTime = Math.Max(tempToken.BornTime, lastBornTime);
            }

            //еасли на всех что-то есть, то убираем их очереди
            for (int i = 0; i < this.InputQuantity; i++)
            {
                var tempToken = inputQueue[i].Dequeue();

                tempToken.ProcessStartTime = tempToken.BornTime;
                tempToken.ProcessEndTime = lastBornTime;
                tempToken.ProcessedByBlock = this;
                collector.Collect(tempToken);
            }

            //выпускаем на выход синтетический
            outputs[0] = new Token(lastBornTime, complexity / this.InputQuantity) { Parent = this };
        }

        public override void Update(ModelingTime modelingTime)
        {
            for(int i=0; i<this.InputQuantity;i++)
            {
                //если на всех блоках чего-нибудь нет, ничего не делаем
                if (inputQueue[i].Count() == 0)
                    return;
            }
            //еасли на всех что-то есть, то это ошибка
            throw new Exception(String.Format("Ошибка блока {0}, блок должен выдывать токены во время работы функции AddToken, а не функции Update", this.Description));
            //for (int i = 0; i < this.InputQuantity; i++)
            //{
            //    var token = inputQueue[i].Dequeue();
            //    token.ProcessEndTime = modelingTime.Now;
            //    collector.Collect(token);
            //    complexity += token.Complexity;
            //}
            //outputs[0] = new Token(modelingTime.Now, complexity/this.InputQuantity) { Parent = this };

           
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
