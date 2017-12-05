using GidraSIM.Model;
using GidraSIM.Model.Procedures;
using GidraSIM.Model.Resources;
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
            double i = 0;
            Token token = null;
            for (i = 0; i <= 10 && token== null; i += 1)
            {
                procedure.Update(i);

                token = procedure.GetOutputToken(0);
                procedure.ClearOutputs();
            }
            if (token == null)
                Console.WriteLine("Test not passed");

            Console.WriteLine("Test passed");
        }
    }
}
