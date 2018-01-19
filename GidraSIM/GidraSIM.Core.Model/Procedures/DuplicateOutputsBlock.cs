using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model.Procedures
{
    [DataContract(IsReference = true)]
    public class DuplicateOutputsBlock : AbstractBlock
    {

        public DuplicateOutputsBlock(int outputsQuantity) : base(1,outputsQuantity)
        {
            Description = "🔱";
        }

        public override void AddToken(Token token, int inputNumber)
        {
            if (inputNumber > 0)
                throw new ArgumentOutOfRangeException();
            //выдаём на выходы токены
            for (int i = 0; i < this.OutputQuantity; i++)
            {
                outputs[i] = new Token(token.BornTime, token.Complexity) { Parent = token.Parent };
            }
        }

        public override void Update(ModelingTime modelingTime)
        {
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
