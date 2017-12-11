using Microsoft.VisualStudio.TestTools.UnitTesting;
using GidraSIM.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Process process = new Process(new TokensCollector());
            Process subprocess = new Process(process.Collector);

            SampleTestingProcedure procedure = new SampleTestingProcedure(subprocess.Collector);
            SampleTestingProcedure procedure2 = new SampleTestingProcedure(process.Collector);

            subprocess.Blocks.Add(procedure);
            subprocess.StartBlock = procedure;
            subprocess.EndBlock = procedure;

            process.Blocks.Add(subprocess);
            process.Blocks.Add(procedure2);
            process.Connections.Connect(subprocess, 0, procedure2, 0);
            process.StartBlock = subprocess;
            process.EndBlock = procedure2;

            Token token = null;


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