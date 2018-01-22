using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ProcedureSaveTest
{
    [TestClass]
    public class PaperworkProcedureSaveTest : IProcedureSaveTest
    {
        [TestMethod]
        public void CreateAndUpdateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            PaperworkProcedure procedure1 = new PaperworkProcedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            procedure1.Update(new ModelingTime());

            PaperworkProcedure procedure2;

            // Act
            procedure2 = SaveTester<PaperworkProcedure>.StartSaveTest(procedure1);

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
            PaperworkProcedure procedure1 = new PaperworkProcedure();
            PaperworkProcedure procedure2;

            // Act
            procedure2 = SaveTester<PaperworkProcedure>.StartSaveTest(procedure1);

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
            IBlock procedure1 = new PaperworkProcedure();
            IBlock procedure2;

            // Act
            procedure2 = SaveTester<IBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1 as PaperworkProcedure).Description, (procedure2 as PaperworkProcedure).Description);
            Assert.AreEqual((procedure1 as PaperworkProcedure).InputQuantity, (procedure2 as PaperworkProcedure).InputQuantity);
            Assert.AreEqual((procedure1 as PaperworkProcedure).OutputQuantity, (procedure2 as PaperworkProcedure).OutputQuantity);
            Assert.AreEqual((procedure1 as PaperworkProcedure).ResourceCount, (procedure2 as PaperworkProcedure).ResourceCount);
            Assert.AreEqual((procedure1 as PaperworkProcedure).TokenCollector, (procedure2 as PaperworkProcedure).TokenCollector);
        }

        [TestMethod]
        public void CreateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            PaperworkProcedure procedure1 = new PaperworkProcedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            PaperworkProcedure procedure2;

            // Act
            procedure2 = SaveTester<PaperworkProcedure>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).ResourceCount, (procedure2).ResourceCount);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }
    }
}
