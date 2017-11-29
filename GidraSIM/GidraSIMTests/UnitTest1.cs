using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GidraSIM.Model;

namespace GidraSIMTests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// basic test
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            TokensCollector tokensCollector = new TokensCollector();
            TestTpprProcedure testTpprProcedure = new TestTpprProcedure(tokensCollector);

            int prevCount = 0;
            int currentCount = -1;
            while (prevCount != currentCount && currentCount < 100)
            {
                testTpprProcedure.Update(1);

                prevCount = currentCount;
                currentCount = tokensCollector.GetHistory().Count;
            }
            if (currentCount != 1)
                Assert.Fail();
        }
        

    }
}
