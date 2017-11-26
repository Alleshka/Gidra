using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model
{
    /// <summary>
    /// токнен, который гуляет по сетке
    /// </summary>
    public class Token
    {
        public double BornTime
        {
            get;
            private set;
        }

        public double ProcessStartTime
        {
            get;
            set;
        }

        public double Complexity
        {
            get;
            private set;
        }

        public double Progress
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public IBlock Parent
        {
            get;
            set;
        }

        public IBlock ProcessedByBlock
        {
            get;
            set;
        }

        public Token(double bornTime, double complexity)
        {
            BornTime = bornTime;
            Complexity = complexity;
            Progress = 0;
        }
    }
}
