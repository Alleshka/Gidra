using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model
{
    /// <summary>
    /// сборщик всех блоков после обработки
    /// </summary>
    [DataContract]
    public class TokensCollector : ITokensCollector, IObjectReference
    {
        [DataMember(EmitDefaultValue = false)] 
        private static TokensCollector tokensCollector = new TokensCollector();

        private TokensCollector()
        {
            history = new List<Token>();
        }

        public static TokensCollector GetInstance()
        {
            return tokensCollector;
        }

        /// <summary>
        /// история всех токенов
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        List<Token> history; /* = new List<Token>();*/

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

        // Это надо для нормальной сериализации синглтона 

        public object GetRealObject(StreamingContext context)
        {
            TokensCollector realObject = GetInstance();
            realObject.Merge(this);
            return realObject;
        }

        private void Merge(TokensCollector otherInstance)
        {
            ////otherInstance.history = this.history;           
            //foreach (var h in otherInstance.history)
            //{
            //    this.history.Add(h);
            //}
            this.history = otherInstance.history;
        }
    }
}
