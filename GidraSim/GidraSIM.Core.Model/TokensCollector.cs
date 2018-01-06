using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Core.Model
{
    /// <summary>
    /// сборщик всех блоков после обработки
    /// </summary>
    public class TokensCollector : ITokensCollector
    {
        private static TokensCollector tokensCollector = new TokensCollector();

        private TokensCollector()
        {

        }

        public static TokensCollector GetInstance()
        {
            return tokensCollector;
        }

        /// <summary>
        /// история всех токенов
        /// </summary>
        List<Token> history = new List<Token>();

        /// <summary>
        /// поместить токен в историю
        /// </summary>
        /// <param name="token"></param>
        public void Collect(Token token)
        {
            history.Add(token);
        }

        /// <summary>
        /// получить доступ к истории
        /// </summary>
        /// <returns></returns>
        public List<Token> GetHistory()
        {
            return history;
        }
    }
}
