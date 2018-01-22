using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ProcedureSaveTest
{
    [TestClass]
    public class ClientCoordinationPrrocedureSaveTest : IProcedureSaveTest
    {
        [TestMethod]
        public void CreateAndUpdateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            ClientCoordinationPrrocedure procedure1 = new ClientCoordinationPrrocedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            procedure1.Update(new ModelingTime());

            ClientCoordinationPrrocedure procedure2;

            // Act
            procedure2 = SaveTester<ClientCoordinationPrrocedure>.StartSaveTest(procedure1);

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
            ClientCoordinationPrrocedure procedure1 = new ClientCoordinationPrrocedure();
            ClientCoordinationPrrocedure procedure2;

            // Act
            procedure2 = SaveTester<ClientCoordinationPrrocedure>.StartSaveTest(procedure1);

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
            IBlock procedure1 = new ClientCoordinationPrrocedure();
            IBlock procedure2;

            // Act
            procedure2 = SaveTester<IBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1 as ClientCoordinationPrrocedure).Description, (procedure2 as ClientCoordinationPrrocedure).Description);
            Assert.AreEqual((procedure1 as ClientCoordinationPrrocedure).InputQuantity, (procedure2 as ClientCoordinationPrrocedure).InputQuantity);
            Assert.AreEqual((procedure1 as ClientCoordinationPrrocedure).OutputQuantity, (procedure2 as ClientCoordinationPrrocedure).OutputQuantity );
            Assert.AreEqual((procedure1 as ClientCoordinationPrrocedure).ResourceCount, (procedure2 as ClientCoordinationPrrocedure).ResourceCount);
            Assert.AreEqual((procedure1 as ClientCoordinationPrrocedure).TokenCollector, (procedure2 as ClientCoordinationPrrocedure).TokenCollector);
        }

        [TestMethod]
        public void CreateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            ClientCoordinationPrrocedure procedure1 = new ClientCoordinationPrrocedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            ClientCoordinationPrrocedure procedure2;

            // Act
            procedure2 = SaveTester<ClientCoordinationPrrocedure>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).ResourceCount, (procedure2).ResourceCount);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }
    }
}
