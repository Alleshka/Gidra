using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ResourceSaveTest
{
    [TestClass]
    public class CadResourceSaveTest : IResourceSaveTest
    {
        [TestMethod]
        public void AsIResourceTest()
        {
            // Arrange 
            IResource cadResource1 = new CadResource();
            IResource cadResource2;

            // Act
            cadResource2 = SaveTester<IResource>.StartSaveTest(cadResource1);

            // Arranges
            Assert.AreEqual((cadResource1 as CadResource).Count, (cadResource2 as CadResource).Count);
            Assert.AreEqual((cadResource1 as CadResource).Description, (cadResource2 as CadResource).Description);
        }

        [TestMethod]
        public void CustomTest()
        {
            // Arrange 
            CadResource cadResource1 = new CadResource()
            {
                Count = 1000,
                Description = "SimSalabimSAPR"
            };

            CadResource cadResource2;

            // Act
            cadResource2 = SaveTester<CadResource>.StartSaveTest(cadResource1);

            // Arranges
            Assert.AreEqual((cadResource1).Count, (cadResource2).Count);
            Assert.AreEqual((cadResource1).Description, (cadResource2).Description);
        }

        [TestMethod]
        public void DefaultTest()
        {
            // Arrange 
            CadResource cadResource1 = new CadResource();
            CadResource cadResource2;

            // Act
            cadResource2 = SaveTester<CadResource>.StartSaveTest(cadResource1);

            // Arranges
            Assert.AreEqual((cadResource1).Count, (cadResource2).Count);
            Assert.AreEqual((cadResource1).Description, (cadResource2).Description);
        }
    }
}
