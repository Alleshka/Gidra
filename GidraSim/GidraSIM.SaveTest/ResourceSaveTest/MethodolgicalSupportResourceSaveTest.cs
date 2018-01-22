using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ResourceSaveTest
{
    [TestClass]
    public class MethodolgicalSupportResourceSaveTest : IResourceSaveTest
    {
        [TestMethod]
        public void AsIResourceTest()
        {
            // Arrange 
            IResource resource1 = new MethodolgicalSupportResource();
            IResource resource2;

            // Act
            resource2 = SaveTester<IResource>.StartSaveTest(resource1);

            // Arranges
            Assert.AreEqual((resource1 as MethodolgicalSupportResource).Description, (resource2 as MethodolgicalSupportResource).Description);
        }

        [TestMethod]
        public void CustomTest()
        {
            // Arrange 
            MethodolgicalSupportResource resource1 = new MethodolgicalSupportResource()
            {
                Description = "ГорячевНовакова"
            };
            MethodolgicalSupportResource resource2;

            // Act
            resource2 = SaveTester<MethodolgicalSupportResource>.StartSaveTest(resource1);

            // Arranges
            Assert.AreEqual((resource1).Description, (resource2).Description);
        }

        [TestMethod]
        public void DefaultTest()
        {
            // Arrange 
            MethodolgicalSupportResource resource1 = new MethodolgicalSupportResource();
            MethodolgicalSupportResource resource2;

            // Act
            resource2 = SaveTester<MethodolgicalSupportResource>.StartSaveTest(resource1);

            // Arranges
            Assert.AreEqual((resource1).Description, (resource2).Description);
        }
    }
}
