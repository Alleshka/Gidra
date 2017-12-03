﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model
{
    /// <summary>
    /// токнен, который гуляет по сетке
    /// представляет собой задачу
    /// </summary>
    public class Token
    {
        /// <summary>
        /// время создания токена блоком
        /// </summary>
        public double BornTime
        {
            get;
            private set;
        }

        /// <summary>
        /// время начала процесса
        /// (в идеале на dt больше времени создания)
        /// </summary>
        public double ProcessStartTime
        {
            get;
            set;
        }

        /// <summary>
        /// время окончания прцоесса
        /// (должно быть больше начала как минимум на dt)
        /// </summary>
        public double ProcessEndTime
        {
            get;
            set;
        }

        /// <summary>
        /// сложность задачи
        /// </summary>
        public double Complexity
        {
            get;
            private set;
        }

        /// <summary>
        /// статус выполнения задачи
        /// 1 - задача выполнена
        /// </summary>
        public double Progress
        {
            get;
            set;
        }
        /// <summary>
        /// описание задачи
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// блок, породивший токен
        /// </summary>
        public IBlock Parent
        {
            get;
            set;
        }

        /// <summary>
        /// блок, обработавший задачу
        /// </summary>
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
    }
}