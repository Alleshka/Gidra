using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model.Procedures;

namespace GidraSIM.Core.Test
{
    [TestClass()]
    public class SchemaCreationProcedureTests
    {
        [TestMethod()]
        public void SchemaCreationProcedureTest()
        {
            SchemaCreationProcedure schemaCreationProcedure = new SchemaCreationProcedure(null);
            /*if (schemaCreationProcedure != null)
                Assert.Fail();*/

            schemaCreationProcedure = new SchemaCreationProcedure(new TokensCollector());
            if (schemaCreationProcedure == null)
                Assert.Fail();
        }

        [TestMethod()]
        public void UpdateTest()
        {
            //подготовка
            SchemaCreationProcedure procedure = new SchemaCreationProcedure(new TokensCollector());
            procedure.AddResorce(new CadResource());
            procedure.AddResorce(new WorkerResource() { Name = "Test", Position = "Работяга", WorkerQualification = WorkerResource.Qualification.SecondCategory });
            procedure.AddResorce(new TechincalSupportResource() { Frequency = 1.5, Ram = 2, Vram = 1 });
            procedure.AddToken(new Token(0, 10), 0);

            //сам тест
            double i = 0;
            Token token = null;
            for (i = 0; i <= 10 && token == null; i += 1)
            {
                procedure.Update(i);

                token = procedure.GetOutputToken(0);
                procedure.ClearOutputs();
            }
            //првоерка выполения процедуры
            if (token == null)
                Assert.Fail();
            //процервка времени выполнения процедуры
                if (i < 9.1 || i > 10.1)//для образцовых моделей со сложностью 10 ремя долнжо быть 10
                    Assert.Fail();

        }
    }
}