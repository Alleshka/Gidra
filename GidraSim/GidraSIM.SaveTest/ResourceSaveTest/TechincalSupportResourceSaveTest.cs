using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;


namespace GidraSIM.SaveTest.ResourceSaveTest
{
    [TestClass]
    public class TechincalSupportResourceSaveTest : IResourceSaveTest
    {
        [TestMethod]
            public void AsIResourceTest()
        {
            // Arrange 
            IResource resource1 = new TechincalSupportResource();
            IResource resource2;

            // Act
            resource2 = SaveTester<IResource>.StartSaveTest(resource1);

            // Arranges
            Assert.AreEqual((resource1 as TechincalSupportResource).Description, (resource2 as TechincalSupportResource).Description);
            Assert.AreEqual((resource1 as TechincalSupportResource).Count, (resource2 as TechincalSupportResource).Count);
            Assert.AreEqual((resource1 as TechincalSupportResource).Frequency, (resource2 as TechincalSupportResource).Frequency);
            Assert.AreEqual((resource1 as TechincalSupportResource).Ram, (resource2 as TechincalSupportResource).Ram);
            Assert.AreEqual((resource1 as TechincalSupportResource).Vram, (resource2 as TechincalSupportResource).Vram);
        }

        [TestMethod]
        public void CustomTest()
        {
            // Arrange 
            TechincalSupportResource resource1 = new TechincalSupportResource()
            {
                Count = 100,
                Description = "Калькулятор",
                Frequency = 100,
                Ram = 10,
                Vram = 10
            };

            TechincalSupportResource resource2;

            // Act
            resource2 = SaveTester<TechincalSupportResource>.StartSaveTest(resource1);

            // Arranges
            Assert.AreEqual((resource1).Description, (resource2).Description);
            Assert.AreEqual((resource1).Count, (resource2).Count);
            Assert.AreEqual((resource1).Frequency, (resource2).Frequency);
            Assert.AreEqual((resource1).Ram, (resource2).Ram);
            Assert.AreEqual((resource1).Vram, (resource2).Vram);
        }

        [TestMethod]
        public void DefaultTest()
        {
            // Arrange 
            TechincalSupportResource resource1 = new TechincalSupportResource();
            TechincalSupportResource resource2;

            // Act
            resource2 = SaveTester<TechincalSupportResource>.StartSaveTest(resource1);

            // Arranges
            Assert.AreEqual((resource1).Description, (resource2).Description);
            Assert.AreEqual((resource1).Count, (resource2).Count);
            Assert.AreEqual((resource1).Frequency, (resource2).Frequency);
            Assert.AreEqual((resource1).Ram, (resource2).Ram);
            Assert.AreEqual((resource1).Vram, (resource2).Vram);
        }
    }
}
