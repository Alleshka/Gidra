using System.Collections.Generic;

namespace GidraSIM.Core.Model
{
    public interface ITokensCollector
    {
        List<Token> GetHistory();
        void Collect(Token token);
    }
}
