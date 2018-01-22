using Microsoft.VisualStudio.TestTools.UnitTesting;
using GidraSIM.Core.Model.Procedures;

namespace GidraSIM.Core.Model.Tests
{
    [TestClass()]
    public class DuplicateOutputsBlockTests
    {

        [TestMethod()]
        public void AddTokenTest()
        {
            #region Тест одновходного блока паралелльности
            DuplicateOutputsBlock duplicateOutputsBlock = new DuplicateOutputsBlock(1);

            duplicateOutputsBlock.AddToken(new Token(0, 0), 0);
            //првоеряем, родил ли блок токен
            Assert.IsNotNull(duplicateOutputsBlock.GetOutputToken(0));
            #endregion


            #region Тест двувходного блока паралелльности
            duplicateOutputsBlock = new DuplicateOutputsBlock(2);

            duplicateOutputsBlock.AddToken(new Token(0, 0), 0);
            //duplicateOutputsBlock.AddToken(new Token(0, 0), 1);
            //првоеряем, родил ли блок токен
            Assert.IsNotNull(duplicateOutputsBlock.GetOutputToken(0));
            Assert.IsNotNull(duplicateOutputsBlock.GetOutputToken(1));
            #endregion
        }
    }
}