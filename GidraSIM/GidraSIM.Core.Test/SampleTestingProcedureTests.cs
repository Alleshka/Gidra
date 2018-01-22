using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GidraSIM.Core.Model.Procedures.Tests
{
    [TestClass()]
    public class SampleTestingProcedureTests
    {
        [TestMethod()]
        public void UpdateWithResTest()
        {
            // arrange 
            SampleTestingProcedure procedure = new SampleTestingProcedure();
            procedure.AddToken(new Token(bornTime: 0, complexity: 1), 0);
            Token token = null;

            // act
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            for (modelingTime.Now = 0; modelingTime.Now < 1000 && token == null; modelingTime.Now += modelingTime.Delta)
            {
                procedure.Update(modelingTime);
                token = procedure.GetOutputToken(0);
                procedure.ClearOutputs();
            }

            // Asserts
            Assert.AreNotEqual(token, null);
            if (modelingTime.Now < 99 || modelingTime.Now > 100) Assert.Fail();//ровно 100 для сложности 1
        }

    }
}