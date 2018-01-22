using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ResourceSaveTest
{
    [TestClass]
    public class WorkerResourceSaveTest : IResourceSaveTest
    {
        [TestMethod]
        public void AsIResourceTest()
        {
            // Arrange 
            IResource resource1 = new WorkerResource();
            IResource resource2;

            // Act
            resource2 = SaveTester<IResource>.StartSaveTest(resource1);

            // Arranges
            Assert.AreEqual((resource1 as WorkerResource).Description, (resource2 as WorkerResource).Description);
            Assert.AreEqual((resource1 as WorkerResource).WorkerQualification, (resource2 as WorkerResource).WorkerQualification);
        }

        [TestMethod]
        public void CustomTest()
        {
            // Arrange 
            WorkerResource resource1 = new WorkerResource()
            {
                Description = "Магомед",
                WorkerQualification = Qualification.ThirdCategory
            };
            WorkerResource resource2;

            // Act
            resource2 = SaveTester<WorkerResource>.StartSaveTest(resource1);

            // Arranges
            Assert.AreEqual((resource1).Description, (resource2).Description);
            Assert.AreEqual((resource1).WorkerQualification, (resource2).WorkerQualification);
        }

        [TestMethod]
        public void DefaultTest()
        {
            // Arrange 
            WorkerResource resource1 = new WorkerResource();
            WorkerResource resource2;

            // Act
            resource2 = SaveTester<WorkerResource>.StartSaveTest(resource1);

            // Arranges
            Assert.AreEqual((resource1).Description, (resource2).Description);
            Assert.AreEqual((resource1).WorkerQualification, (resource2).WorkerQualification);
        }
    }
}
