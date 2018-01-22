using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.SaveTest
{
    [TestClass]
    public class ProjectSaveTest
    {
        [TestMethod]
        public void CreateProcessTest()
        {
            // Arrange 
            Process process1 = new Process();

            SampleTestingProcedure procedure1 = new SampleTestingProcedure();
            SampleTestingProcedure procedure2 = new SampleTestingProcedure();

            process1.Blocks.Add(procedure1);
            process1.Blocks.Add(procedure2);

            process1.Connections.Connect(procedure1, 0, procedure2, 0);

            process1.StartBlock = procedure1;
            process1.EndBlock = procedure2;

            // act
            Process process2 = SaveTester<Process>.StartSaveTest(process1);

            // Asserts
            Assert.AreEqual(process1, process2);
        }


        [TestMethod]
        public void CreateProcessWithResTest()
        {
            // Arrange 
            Process process1 = new Process();

            SampleTestingProcedure procedure1 = new SampleTestingProcedure();
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new TechincalSupportResource());

            SampleTestingProcedure procedure2 = new SampleTestingProcedure();

            process1.Blocks.Add(procedure1);
            process1.Blocks.Add(procedure2);

            process1.Connections.Connect(procedure1, 0, procedure2, 0);

            process1.StartBlock = procedure1;
            process1.EndBlock = procedure2;

            // act
            Process process2 = SaveTester<Process>.StartSaveTest(process1);

            // Asserts
            Assert.AreEqual(process1, process2);
        }

        [TestMethod]
        public void StartProcess()
        {
            // Arrange 
            Process process1 = new Process();

            SampleTestingProcedure procedure1 = new SampleTestingProcedure();
            procedure1.AddResorce(new CadResource());
            procedure1.AddResorce(new WorkerResource());
            procedure1.AddResorce(new TechincalSupportResource());

            SampleTestingProcedure procedure2 = new SampleTestingProcedure();

            process1.Blocks.Add(procedure1);
            process1.Blocks.Add(procedure2);

            process1.Connections.Connect(procedure1, 0, procedure2, 0);

            process1.StartBlock = procedure1;
            process1.EndBlock = procedure2;

            //добавляем на стартовый блок токен
            process1.AddToken(new Token(0, complexity: 1), 0);
            //double i = 0;
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            //цикл до тех пор, пока на выходе не появится токен
            for (modelingTime.Now = 0; modelingTime.Now < 1000 && !process1.EndBlockHasOutputToken; modelingTime.Now += modelingTime.Delta)
            {
                process1.Update(modelingTime);
            }

            // act
            Process process2 = SaveTester<Process>.StartSaveTest(process1);

            // Asserts
            Assert.AreEqual(process1, process2);
        }

        [TestMethod]
        public void SaveSubProcessTest()
        {
            Process process = new Process();
            Process subprocess = new Process();

            TracingProcedure procedure = new TracingProcedure();
            procedure.AddResorce(new CadResource());
            procedure.AddResorce(new WorkerResource());
            procedure.AddResorce(new TechincalSupportResource());

            ArrangementProcedure procedure2 = new ArrangementProcedure();
            procedure2.AddResorce(new CadResource());
            procedure2.AddResorce(new WorkerResource());
            procedure2.AddResorce(new TechincalSupportResource());

            subprocess.Blocks.Add(procedure);
            subprocess.Blocks.Add(procedure2);
            subprocess.Connections.Connect(procedure, 0, procedure2, 0);

            subprocess.StartBlock = procedure;
            subprocess.EndBlock = procedure2;

            process.Blocks.Add(subprocess);
            process.StartBlock = subprocess;
            process.EndBlock = subprocess;


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

            Process process2 = SaveTester<Process>.StartSaveTest(process);
            // Asserts
            Assert.AreEqual(process, process2);
        }
    }
}