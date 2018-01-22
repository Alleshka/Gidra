using Microsoft.VisualStudio.TestTools.UnitTesting;
using GidraSIM.Core.Model.Procedures;

namespace GidraSIM.Core.Model.Tests
{
    [TestClass()]
    public class AndBlockTests
    {

        [TestMethod()]
        public void AddTokenTest()
        {
            #region Тест одновходного блока и
            AndBlock andBlock = new AndBlock(1);

            
            andBlock.AddToken(new Token(0, 0), 0);
            //првоеряем, родил ли блок токен
            Assert.IsNotNull(andBlock.GetOutputToken(0));
            #endregion

            #region Тест двувходного блока 
            andBlock = new AndBlock(2);
            andBlock.AddToken(new Token(1, 2), 0);
            //не должен был родить
            Assert.IsNull(andBlock.GetOutputToken(0));

            
            andBlock.AddToken(new Token(2, 5), 1);
            var token = andBlock.GetOutputToken(0);
            //должен был родить
            Assert.IsNotNull(token);

            //времмя рождения должно быть как последнее из всех входных, т.е. 2
            Assert.AreEqual(2, token.BornTime, 0.1);
            //сложность должна быть средним арифмектическим
            Assert.AreEqual((2+5)/2.0, token.Complexity, 0.1);

            TokensCollector tokensCollector = TokensCollector.GetInstance();
            var collectorToken1 = tokensCollector.GetHistory()[1];
            var collectorToken2 = tokensCollector.GetHistory()[2];
            //в коллектор должны были прийти входные
            Assert.IsNotNull(collectorToken1);
            Assert.IsNotNull(collectorToken2);

            //время обработки должно быть как последнее время рождения
            Assert.AreEqual(token.BornTime, collectorToken1.ProcessEndTime, 0.1);
            Assert.AreEqual(token.BornTime, collectorToken2.ProcessEndTime, 0.1);
            #endregion
        }
    }
}