using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ProcedureSaveTest
{
    [TestClass]
    public class SchemaCreationProcedureSaveTest : IProcedureSaveTest
    {
        [TestMethod]
        public void CreateAndUpdateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            SchemaCreationProcedure procedure1 = new SchemaCreationProcedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            procedure1.Update(new ModelingTime());

            SchemaCreationProcedure procedure2;

            // Act
            procedure2 = SaveTester<SchemaCreationProcedure>.StartSaveTest(procedure1);

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
            SchemaCreationProcedure procedure1 = new SchemaCreationProcedure();
            SchemaCreationProcedure procedure2;

            // Act
            procedure2 = SaveTester<SchemaCreationProcedure>.StartSaveTest(procedure1);

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
            IBlock procedure1 = new SchemaCreationProcedure();
            IBlock procedure2;

            // Act
            procedure2 = SaveTester<IBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1 as SchemaCreationProcedure).Description, (procedure2 as SchemaCreationProcedure).Description);
            Assert.AreEqual((procedure1 as SchemaCreationProcedure).InputQuantity, (procedure2 as SchemaCreationProcedure).InputQuantity);
            Assert.AreEqual((procedure1 as SchemaCreationProcedure).OutputQuantity, (procedure2 as SchemaCreationProcedure).OutputQuantity);
            Assert.AreEqual((procedure1 as SchemaCreationProcedure).ResourceCount, (procedure2 as SchemaCreationProcedure).ResourceCount);
            Assert.AreEqual((procedure1 as SchemaCreationProcedure).TokenCollector, (procedure2 as SchemaCreationProcedure).TokenCollector);
        }

        [TestMethod]
        public void CreateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            SchemaCreationProcedure procedure1 = new SchemaCreationProcedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            SchemaCreationProcedure procedure2;

            // Act
            procedure2 = SaveTester<SchemaCreationProcedure>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).ResourceCount, (procedure2).ResourceCount);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }
    }
}
