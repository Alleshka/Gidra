using Microsoft.VisualStudio.TestTools.UnitTesting;
using GidraSIM.Core.Model.Resources;

namespace GidraSIM.Core.Model.Procedures.Tests
{
    [TestClass()]
    public class FormingDocumentationProcedureTests
    {

        [TestMethod()]
        public void UpdateWithResTest()
        {
            // arrange 
            FormingDocumentationProcedure procedure = new FormingDocumentationProcedure();
            procedure.AddResorce(new WorkerResource()
            {
                //Name = "Alleshka",
                //Position = "Работяга",
                WorkerQualification = Qualification.FirstCategory
            });
            procedure.AddResorce(new TechincalSupportResource()
            {
                Frequency = 1.5,
                Ram = 2,
                Vram = 1
            });
            procedure.AddToken(new Token(bornTime: 0, complexity: 1000), 0);
            Token token = null;

            // act
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            for (modelingTime.Now = 0; modelingTime.Now < 40 && token == null; modelingTime.Now += modelingTime.Delta)
            {
                procedure.Update(modelingTime);
                token = procedure.GetOutputToken(0);
                procedure.ClearOutputs();
            }

            // Asserts
            Assert.AreNotEqual(token, null);
            if (modelingTime.Now < 1 || modelingTime.Now > 31) Assert.Fail();
        }
    }
}