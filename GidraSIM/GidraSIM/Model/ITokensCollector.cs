using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Model
{
    public interface ITokensCollector
    {
        List<Token> GetHistory();
        void Collect(Token token);

        double GlobalTime
        {
            get; set;
        }
    }
}
