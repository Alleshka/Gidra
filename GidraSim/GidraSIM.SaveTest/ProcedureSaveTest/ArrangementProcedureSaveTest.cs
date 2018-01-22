using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ProcedureSaveTest
{
    [TestClass]
    public class ArrangementProcedureSaveTest : IProcedureSaveTest
    {
        [TestMethod]
        public void CreateAndUpdateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            ArrangementProcedure procedure1 = new ArrangementProcedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            procedure1.Update(new ModelingTime());

            ArrangementProcedure procedure2;

            // Act
            procedure2 = SaveTester<ArrangementProcedure>.StartSaveTest(procedure1);

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
            ArrangementProcedure procedure1 = new ArrangementProcedure();
            ArrangementProcedure procedure2;

            // Act
            procedure2 = SaveTester<ArrangementProcedure>.StartSaveTest(procedure1);

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
            IBlock procedure1 = new ArrangementProcedure();
            IBlock procedure2;

            // Act
            procedure2 = SaveTester<IBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1 as ArrangementProcedure).Description, (procedure2 as ArrangementProcedure).Description);
            Assert.AreEqual((procedure1 as ArrangementProcedure).InputQuantity, (procedure2 as ArrangementProcedure).InputQuantity);
            Assert.AreEqual((procedure1 as ArrangementProcedure).OutputQuantity, (procedure2 as ArrangementProcedure).OutputQuantity);
            Assert.AreEqual((procedure1 as ArrangementProcedure).ResourceCount, (procedure2 as ArrangementProcedure).ResourceCount);
            Assert.AreEqual((procedure1 as ArrangementProcedure).TokenCollector, (procedure2 as ArrangementProcedure).TokenCollector);
        }

        [TestMethod]
        public void CreateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            ArrangementProcedure procedure1 = new ArrangementProcedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());     
            ArrangementProcedure procedure2;

            // Act
            procedure2 = SaveTester<ArrangementProcedure>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).ResourceCount, (procedure2).ResourceCount);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }
    }
}
