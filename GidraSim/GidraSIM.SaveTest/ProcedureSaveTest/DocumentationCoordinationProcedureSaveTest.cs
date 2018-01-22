using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ProcedureSaveTest
{
    [TestClass]
    public class DocumentationCoordinationProcedureSaveTest : IProcedureSaveTest
    {
        [TestMethod]
        public void CreateAndUpdateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            DocumentationCoordinationProcedure procedure1 = new DocumentationCoordinationProcedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            procedure1.Update(new ModelingTime());

            DocumentationCoordinationProcedure procedure2;

            // Act
            procedure2 = SaveTester<DocumentationCoordinationProcedure>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).ResourceCount, (procedure2).ResourceCount);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }

        [TestMethod]
        public void CreateEmptyProcedure()
        {
            // Arrange
            DocumentationCoordinationProcedure procedure1 = new DocumentationCoordinationProcedure();
            DocumentationCoordinationProcedure procedure2;

            // Act
            procedure2 = SaveTester<DocumentationCoordinationProcedure>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).ResourceCount, (procedure2).ResourceCount);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }

        [TestMethod]
        public void CreateEmptyProcedureAsIBlock()
        {
            // Arrange
            IBlock procedure1 = new DocumentationCoordinationProcedure();
            IBlock procedure2;

            // Act
            procedure2 = SaveTester<IBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1 as DocumentationCoordinationProcedure).Description, (procedure2 as DocumentationCoordinationProcedure).Description);
            Assert.AreEqual((procedure1 as DocumentationCoordinationProcedure).InputQuantity, (procedure2 as DocumentationCoordinationProcedure).InputQuantity);
            Assert.AreEqual((procedure1 as DocumentationCoordinationProcedure).OutputQuantity, (procedure2 as DocumentationCoordinationProcedure).OutputQuantity);
            Assert.AreEqual((procedure1 as DocumentationCoordinationProcedure).ResourceCount, (procedure2 as DocumentationCoordinationProcedure).ResourceCount);
            Assert.AreEqual((procedure1 as DocumentationCoordinationProcedure).TokenCollector, (procedure2 as DocumentationCoordinationProcedure).TokenCollector);
        }

        [TestMethod]
        public void CreateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            DocumentationCoordinationProcedure procedure1 = new DocumentationCoordinationProcedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            DocumentationCoordinationProcedure procedure2;

            // Act
            procedure2 = SaveTester<DocumentationCoordinationProcedure>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).ResourceCount, (procedure2).ResourceCount);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }
    }
}
