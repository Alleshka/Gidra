using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            SchemaCreationProcedure procedure = new SchemaCreationProcedure(new TokensCollector());
            procedure.AddResorce(new CadResource());
            procedure.AddResorce(new WorkerResource() { Name = "Test", Position = "Работяга", WorkerQualification = WorkerResource.Qualification.SecondCategory });
            procedure.AddResorce(new TechincalSupportResource() { Frequency = 1.5, Ram = 2, Vram = 1 });
            procedure.AddToken(new Token(0, 10), 0);
            Token token = null;
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            for (modelingTime.Now = 0; modelingTime.Now <= 10 && token== null; modelingTime.Now += modelingTime.Delta)
            {
                procedure.Update(modelingTime);

                token = procedure.GetOutputToken(0);
                procedure.ClearOutputs();
            }
            if (token == null)
                Console.WriteLine("Test not passed");

            Console.WriteLine("Test passed");
        }
    }
}
