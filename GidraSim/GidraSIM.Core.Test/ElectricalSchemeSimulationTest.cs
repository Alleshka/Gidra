using Microsoft.VisualStudio.TestTools.UnitTesting;
using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.Core.Test
{
    /// <summary>
    /// Сводное описание для UnitTest1
    /// </summary>
    [TestClass]
    public class ElectricalSchemeSimulationTest
    {
        [TestMethod]
        public void TestWithRes()
        {
            // arrange
            ElectricalSchemeSimulation electricalScheme = new ElectricalSchemeSimulation();
            electricalScheme.AddToken(new Token(bornTime: 0, complexity: 100), 0);

            electricalScheme.AddResorce(new WorkerResource()
            {
                //Name = "Alleshka",
                //Position = "Работяга",
                WorkerQualification = Qualification.SecondCategory
            });


            electricalScheme.AddResorce(new TechincalSupportResource()
            {
                Frequency = 1.5,
                Ram = 2,
                Vram = 1
            });

            electricalScheme.AddResorce(new CadResource());

            Token token = null;

            // act
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            for (modelingTime.Now = 0; modelingTime.Now < 100 && token == null; modelingTime.Now += modelingTime.Delta)
            {
                electricalScheme.Update(modelingTime);
                token = electricalScheme.GetOutputToken(0);
                electricalScheme.ClearOutputs();
            }

            // Asserts
            Assert.AreNotEqual(token, null);
            if (modelingTime.Now < 9.99 || modelingTime.Now > 10.01)//для образцовых моделей со сложностью 100 ремя долнжо быть 10
                Assert.Fail();
        }
    }
}
