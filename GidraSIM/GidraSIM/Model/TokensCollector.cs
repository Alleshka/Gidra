using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Model
{
    public class TokensCollector : ITokensCollector
    {
        List<Token> history = new List<Token>();

        public double GlobalTime
        {
            get; set;
        }

        public void Collect(Token token)
        {
            history.Add(token);
        }

        public List<Token> GetHistory()
        {
            return history;
        }
    }
}
