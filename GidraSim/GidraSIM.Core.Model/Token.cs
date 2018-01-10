using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model
{
    /// <summary>
    /// токнен, который гуляет по сетке
    /// представляет собой задачу
    /// </summary>
    [DataContract(IsReference = true)]
    public class Token
    {
        /// <summary>
        /// время создания токена блоком
        /// </summary>
        [DataMember]
        public double BornTime
        {
            get;
            private set;
        }

        /// <summary>
        /// время начала процесса
        /// (в идеале на dt больше времени создания)
        /// </summary>
        [DataMember]
        public double ProcessStartTime
        {
            get;
            set;
        }

        /// <summary>
        /// время окончания прцоесса
        /// (должно быть больше начала как минимум на dt)
        /// </summary>
        [DataMember]
        public double ProcessEndTime
        {
            get;
            set;
        }

        /// <summary>
        /// сложность задачи
        /// </summary>
        [DataMember]
        public double Complexity
        {
            get;
            private set;
        }

        /// <summary>
        /// статус выполнения задачи
        /// 1 - задача выполнена
        /// </summary>
        [DataMember]
        public double Progress
        {
            get;
            set;
        }
        /// <summary>
        /// описание задачи
        /// </summary>
        [DataMember]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// блок, породивший токен
        /// </summary>
        [DataMember]
        public IBlock Parent
        {
            get;
            set;
        }

        /// <summary>
        /// блок, обработавший задачу
        /// </summary>
        [DataMember]
        public IBlock ProcessedByBlock
        {
            get;
            set;
        }

        /// <summary>
        /// чтобы не забыть, сразу задаётся врем рождения токена
        /// </summary>
        /// <param name="bornTime"></param>
        /// <param name="complexity"></param>
        public Token(double bornTime, double complexity)
        {
            BornTime = bornTime;
            Complexity = complexity;
            Progress = 0;
        }

        public override string ToString()
        {
            return String.Format("Token:born time={0},start time= {1},end time= {2}," +
                "compl={3}, descr={4}, by {5},progr = {6},parent={7}",
                this.BornTime,
                this.ProcessStartTime,
                this.ProcessEndTime,
                this.Complexity,
                this.Description,
                this.ProcessedByBlock,
                this.Progress,
                this.Parent);
        }

        public override bool Equals(object obj)
        {
            Token temp = obj as Token;

            if (temp.ProcessEndTime != this.ProcessEndTime)
                return false;
            if (temp.ProcessStartTime != this.ProcessStartTime)
                return false;
            if (temp.Progress != this.Progress)
                return false;

            if ((!Equals(temp.Parent, this.Parent)))
                return false;
            if ((!Equals(temp.ProcessedByBlock, this.ProcessedByBlock)))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
