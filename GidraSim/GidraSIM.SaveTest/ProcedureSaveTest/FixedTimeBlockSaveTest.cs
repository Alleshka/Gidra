using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ProcedureSaveTest
{
    [TestClass]
    public class FixedTimeBlockSaveTest : IProcedureSaveTest
    {
        [TestMethod]
        public void CreateAndUpdateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            FixedTimeBlock procedure1 = new FixedTimeBlock(100);
            procedure1.AddToken(temp, 0);
            procedure1.Update(new ModelingTime());

            FixedTimeBlock procedure2;

            // Act
            procedure2 = SaveTester<FixedTimeBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).FixedTime, (procedure2).FixedTime);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }

        [TestMethod]
        public void CreateEmptyProcedure()
        {
            // Arrange
            FixedTimeBlock procedure1 = new FixedTimeBlock(100);
            FixedTimeBlock procedure2;

            // Act
            procedure2 = SaveTester<FixedTimeBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).FixedTime, (procedure2).FixedTime);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }

        [TestMethod]
        public void CreateEmptyProcedureAsIBlock()
        {
            // Arrange
            IBlock procedure1 = new FixedTimeBlock(100);
            IBlock procedure2;

            // Act
            procedure2 = SaveTester<IBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1 as FixedTimeBlock).Description, (procedure2 as FixedTimeBlock).Description);
            Assert.AreEqual((procedure1 as FixedTimeBlock).InputQuantity, (procedure2 as FixedTimeBlock).InputQuantity);
            Assert.AreEqual((procedure1 as FixedTimeBlock).OutputQuantity, (procedure2 as FixedTimeBlock).OutputQuantity);
            Assert.AreEqual((procedure1 as FixedTimeBlock).FixedTime, (procedure2 as FixedTimeBlock).FixedTime);
            Assert.AreEqual((procedure1 as FixedTimeBlock).TokenCollector, (procedure2 as FixedTimeBlock).TokenCollector);
        }

        [TestMethod]
        public void CreateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            FixedTimeBlock procedure1 = new FixedTimeBlock(100);
            procedure1.AddToken(temp, 0);
            FixedTimeBlock procedure2;

            // Act
            procedure2 = SaveTester<FixedTimeBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).FixedTime, (procedure2).FixedTime);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }
    }
}
