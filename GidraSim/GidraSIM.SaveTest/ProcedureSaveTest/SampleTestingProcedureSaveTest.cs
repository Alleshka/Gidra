using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest.ProcedureSaveTest
{
    [TestClass]
    public class SampleTestingProcedureSaveTest : IProcedureSaveTest
    {
        [TestMethod]
        public void CreateAndUpdateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            SampleTestingProcedure procedure1 = new SampleTestingProcedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            procedure1.Update(new ModelingTime());

            SampleTestingProcedure procedure2;

            // Act
            procedure2 = SaveTester<SampleTestingProcedure>.StartSaveTest(procedure1);

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
            SampleTestingProcedure procedure1 = new SampleTestingProcedure();
            SampleTestingProcedure procedure2;

            // Act
            procedure2 = SaveTester<SampleTestingProcedure>.StartSaveTest(procedure1);

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
            IBlock procedure1 = new SampleTestingProcedure();
            IBlock procedure2;

            // Act
            procedure2 = SaveTester<IBlock>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1 as SampleTestingProcedure).Description, (procedure2 as SampleTestingProcedure).Description);
            Assert.AreEqual((procedure1 as SampleTestingProcedure).InputQuantity, (procedure2 as SampleTestingProcedure).InputQuantity);
            Assert.AreEqual((procedure1 as SampleTestingProcedure).OutputQuantity, (procedure2 as SampleTestingProcedure).OutputQuantity);
            Assert.AreEqual((procedure1 as SampleTestingProcedure).ResourceCount, (procedure2 as SampleTestingProcedure).ResourceCount);
            Assert.AreEqual((procedure1 as SampleTestingProcedure).TokenCollector, (procedure2 as SampleTestingProcedure).TokenCollector);
        }

        [TestMethod]
        public void CreateProcedure()
        {
            // Arrange
            Token temp = new Token(10, 15);

            SampleTestingProcedure procedure1 = new SampleTestingProcedure();
            procedure1.AddToken(temp, 0);
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new TechincalSupportResource());
            SampleTestingProcedure procedure2;

            // Act
            procedure2 = SaveTester<SampleTestingProcedure>.StartSaveTest(procedure1);

            // Asserts
            Assert.AreEqual((procedure1).Description, (procedure2).Description);
            Assert.AreEqual((procedure1).InputQuantity, (procedure2).InputQuantity);
            Assert.AreEqual((procedure1).OutputQuantity, (procedure2).OutputQuantity);
            Assert.AreEqual((procedure1).ResourceCount, (procedure2).ResourceCount);
            Assert.AreEqual((procedure1).TokenCollector, (procedure2).TokenCollector);
        }
    }
}
