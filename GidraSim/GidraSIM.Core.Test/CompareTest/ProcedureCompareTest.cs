using Microsoft.VisualStudio.TestTools.UnitTesting;

using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model;

namespace GidraSIM.Core.Test.CompareTest
{
    [TestClass]
    public class ProcedureCompareTest
    {
        [TestMethod]
        public void TracingProcedureTestTrue()
        {
            // Arrange
            TracingProcedure tracing1 = new TracingProcedure();
            tracing1.AddResorce(new WorkerResource() { WorkerQualification = Qualification.SecondCategory, Description = "DahaYvolena"});
            tracing1.AddResorce(new CadResource());
            tracing1.AddResorce(new TechincalSupportResource());

            TracingProcedure tracing21 = new TracingProcedure();
            tracing21.AddResorce(new WorkerResource() { WorkerQualification = Qualification.SecondCategory, Description = "DahaYvolena" });
            tracing21.AddResorce(new CadResource());
            tracing21.AddResorce(new TechincalSupportResource());

            // Asserts
            Assert.AreEqual(tracing1, tracing21);
        }

        [TestMethod]
        public void TracingProcedureTestFalse()
        {
            // Arrange
            TracingProcedure tracing1 = new TracingProcedure();
            tracing1.AddResorce(new WorkerResource() { WorkerQualification = Qualification.SecondCategory, Description = "DahaYvolena" });
            tracing1.AddResorce(new CadResource());
            tracing1.AddResorce(new TechincalSupportResource());

            TracingProcedure tracing21 = new TracingProcedure();
            tracing21.AddResorce(new WorkerResource() { WorkerQualification = Qualification.SecondCategory, Description = "DahaJunior" });
            tracing21.AddResorce(new CadResource());
            tracing21.AddResorce(new TechincalSupportResource());

            // Asserts
            Assert.AreNotEqual(tracing1, tracing21);
        }

        [TestMethod]
        public void ProcessCompareTestTrue()
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

            Process process2 = new Process();

            SampleTestingProcedure procedure21 = new SampleTestingProcedure();
            SampleTestingProcedure procedure22 = new SampleTestingProcedure();

            process2.Blocks.Add(procedure21);
            process2.Blocks.Add(procedure22);

            process2.Connections.Connect(procedure21, 0, procedure22, 0);

            process2.StartBlock = procedure21;
            process2.EndBlock = procedure22;
        }


        [TestMethod]
        public void ProcessCompareTestFalse()
        {
            // Arrange 
            Process process1 = new Process();

            TracingProcedure procedure1 = new TracingProcedure();
            SampleTestingProcedure procedure2 = new SampleTestingProcedure();

            process1.Blocks.Add(procedure1);
            process1.Blocks.Add(procedure2);

            process1.Connections.Connect(procedure1, 0, procedure2, 0);

            process1.StartBlock = procedure1;
            process1.EndBlock = procedure2;

            Process process2 = new Process();

            SampleTestingProcedure procedure21 = new SampleTestingProcedure();
            SampleTestingProcedure procedure22 = new SampleTestingProcedure();

            process2.Blocks.Add(procedure21);
            process2.Blocks.Add(procedure22);

            process2.Connections.Connect(procedure21, 0, procedure22, 0);

            process2.StartBlock = procedure21;
            process2.EndBlock = procedure22;

            Assert.AreNotEqual(process1, process2);
        }
    }
}
