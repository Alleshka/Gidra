using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Resources;

namespace GidraSIM.Core.Test.CompareTest
{
    [TestClass]
    public class ResourceCompareTest
    {
        [TestMethod]
        public void CadResourceCompare()
        {
            // Arrange
            CadResource cadResource1 = new CadResource()
            {
                Count = 100,
                Description = "СИМСАПР"
            };

            CadResource cadResource2 = cadResource1;

            //Assert
            Assert.AreEqual(cadResource1, cadResource2);
            if (cadResource1 != cadResource2) Assert.Fail();
        }

        [TestMethod]
        public void CadResourceCompareTrue()
        {
            // Arrange
            CadResource cadResource1 = new CadResource()
            {
                Count = 100,
                Description = "СИМСАПР"
            };

            CadResource cadResource2 = new CadResource()
            {
                Count = 100,
                Description = "СИМСАПР"
            };

            //Assert
            Assert.AreEqual(cadResource1, cadResource2);
        }

        [TestMethod]
        public void CadResourceCompareFalseCount()
        {
            // Arrange
            CadResource cadResource1 = new CadResource()
            {
                Count = 100,
                Description = "СИМСАПР"
            };

            CadResource cadResource2 = new CadResource()
            {
                Count = 1000,
                Description = "СИМСАПР"
            };

            //Assert
            Assert.AreNotEqual(cadResource1, cadResource2);
            if (cadResource1 == cadResource2) Assert.Fail();
        }

        [TestMethod]
        public void CadResourceCompareFalseDescription()
        {
            // Arrange
            CadResource cadResource1 = new CadResource()
            {
                Count = 100,
                Description = "СИМСАПР"
            };

            CadResource cadResource2 = new CadResource()
            {
                Count = 100,
                Description = "СИМСАПР1"
            };

            //Assert
            Assert.AreNotEqual(cadResource1, cadResource2);
            if (cadResource1 == cadResource2) Assert.Fail();
        }


        [TestMethod]
        public void CadResourceCompareNULL()
        {
            // Arrange
            CadResource cadResource1 = null;
            CadResource cadResource2 = null;

            //Assert
            Assert.AreEqual(cadResource1, cadResource2);
            if (cadResource1 != cadResource2) Assert.Fail();
        }
    }
}
