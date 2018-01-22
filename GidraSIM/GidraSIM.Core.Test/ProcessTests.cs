using Microsoft.VisualStudio.TestTools.UnitTesting;
using GidraSIM.Core.Model.Procedures;

namespace GidraSIM.Core.Model.Tests
{
    [TestClass()]
    public class ProcessTests
    {
        [TestMethod()]
        public void SubprocessTest()
        {
            //arrange
            Process process = new Process();
            Process subprocess = new Process();

            SampleTestingProcedure procedure = new SampleTestingProcedure();
            SampleTestingProcedure procedure2 = new SampleTestingProcedure();

            subprocess.Blocks.Add(procedure);
            subprocess.StartBlock = procedure;
            subprocess.EndBlock = procedure;

            process.Blocks.Add(subprocess);
            process.Blocks.Add(procedure2);
            process.Connections.Connect(subprocess, 0, procedure2, 0);
            process.StartBlock = subprocess;
            process.EndBlock = procedure2;



            //act

            //добавляем на стартовый блок токен
            process.AddToken(new Token(0, complexity: 1), 0);
            //double i = 0;
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            //цикл до тех пор, пока на выходе не появится токен
            for (modelingTime.Now = 0; modelingTime.Now < 1000 && !process.EndBlockHasOutputToken; modelingTime.Now += modelingTime.Delta)
            {
                process.Update(modelingTime);
            }

            // Asserts
            if (modelingTime.Now < 197 || modelingTime.Now > 200)
                Assert.Fail();
        }
    }
}