using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model
{
    interface IBlock
    {
        void AddToken(Token token, int inputNumber);
        bool AllInputesFilled();

        int OutputQuantity { get;}
        int InputQuantity { get;}
        void Connect(int outputNumber, IBlock block, int blockInputNumber);
        void Update(double dt);
    }
}
