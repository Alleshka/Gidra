using Microsoft.VisualStudio.TestTools.UnitTesting;
using GidraSIM.Core.Model.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model.Procedures.Tests
{
    [TestClass()]
    public class DocumentationCoordinationProcedureTests
    {
        [TestMethod()]
        public void UpdateWithResTest()
        {
            // arrange 
            DocumentationCoordinationProcedure procedure = new DocumentationCoordinationProcedure(new TokensCollector());
            procedure.AddToken(new Token(bornTime: 0, complexity: 1000), 0);
            Token token = null;

            // act
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            for (modelingTime.Now = 0; modelingTime.Now < 80 && token == null; modelingTime.Now += modelingTime.Delta)
            {
                procedure.Update(modelingTime);
                token = procedure.GetOutputToken(0);
                procedure.ClearOutputs();
            }

            // Asserts
            Assert.AreNotEqual(token, null);
            if (modelingTime.Now < 7 || modelingTime.Now > 61) Assert.Fail();
        }
    }
}