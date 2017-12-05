﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Token token = null;
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            for ( modelingTime.Now= 0; modelingTime.Now <= 10 && token == null; modelingTime.Now += modelingTime.Delta)
            {
                procedure.Update(modelingTime);

                token = procedure.GetOutputToken(0);
                procedure.ClearOutputs();
            }
            //првоерка выполения процедуры
            if (token == null)
                Assert.Fail();
            //процервка времени выполнения процедуры
                if (modelingTime.Now < 9.1 || modelingTime.Now > 10.1)//для образцовых моделей со сложностью 10 ремя долнжо быть 10
                    Assert.Fail();

        }
    }
}