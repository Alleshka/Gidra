using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ProcedureSaveTest
{
    [TestClass]
    public class ElectricalSchemeSimulationSaveTest : IProcedureSaveTest
    {
        [TestMethod]
        public void CreateAndUpdateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            ElectricalSchemeSimulation procedure1 = new ElectricalSchemeSimulation();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            procedure1.Update(new ModelingTime());

            ElectricalSchemeSimulation procedure2;

            // Act
            procedure2 = SaveTester<ElectricalSchemeSimulation>.StartSaveTest(procedure1);

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
            ElectricalSchemeSimulation procedure1 = new ElectricalSchemeSimulation();
            ElectricalSchemeSimulation procedure2;

            // Act
            procedure2 = SaveTester<ElectricalSchemeSimulation>.StartSaveTest(procedure1);

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
            IBlock procedure1 = new ElectricalSchemeSimulation();
            IBlock procedure2;

            // Act
            procedure2 = SaveTester<IBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1 as ElectricalSchemeSimulation).Description, (procedure2 as ElectricalSchemeSimulation).Description);
            Assert.AreEqual((procedure1 as ElectricalSchemeSimulation).InputQuantity, (procedure2 as ElectricalSchemeSimulation).InputQuantity);
            Assert.AreEqual((procedure1 as ElectricalSchemeSimulation).OutputQuantity, (procedure2 as ElectricalSchemeSimulation).OutputQuantity);
            Assert.AreEqual((procedure1 as ElectricalSchemeSimulation).ResourceCount, (procedure2 as ElectricalSchemeSimulation).ResourceCount);
            Assert.AreEqual((procedure1 as ElectricalSchemeSimulation).TokenCollector, (procedure2 as ElectricalSchemeSimulation).TokenCollector);
        }

        [TestMethod]
        public void CreateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            ElectricalSchemeSimulation procedure1 = new ElectricalSchemeSimulation();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            ElectricalSchemeSimulation procedure2;

            // Act
            procedure2 = SaveTester<ElectricalSchemeSimulation>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).ResourceCount, (procedure2).ResourceCount);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }
    }
}
