using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.Core.Test
{
    [TestClass]
    public class PaperworkProcedureTest
    {
        [TestMethod]
        public void InitPaperWorkProcdeure()
        {
            // arrange 
            PaperworkProcedure paperworkProcedure = new PaperworkProcedure(new TokensCollector());

            // act 

            // Asserts 
            Assert.AreNotEqual(paperworkProcedure, null);
        }

        [TestMethod]
        public void UpdateWithoutRes()
        {
            // arrange 
            PaperworkProcedure paperworkProcedure = new PaperworkProcedure(new TokensCollector());
            paperworkProcedure.AddToken(new Token(0, 10), 0);
            bool tes = false;

            // act 
            try
            {
                paperworkProcedure.Update(0);
            }
            catch (ArgumentNullException)
            {
                tes = true;
            }

            // Asserts
            Assert.AreEqual(tes, true);
        }

        [TestMethod]
        public void UpdateWithRes()
        {
            // arrange 
                PaperworkProcedure paperworkProcedure = new PaperworkProcedure(new TokensCollector());
            paperworkProcedure.AddResorce(new WorkerResource()
            {
                Name = "Alleshka",
                Position = "Работяга",
                WorkerQualification = WorkerResource.Qualification.FirstCategory
            });
            paperworkProcedure.AddResorce(new TechincalSupportResource()
            {
                Frequency = 1.5,
                Ram = 2,
                Vram = 1
            });
            paperworkProcedure.AddToken(new Token(0, 10), 0);
            Token token = null;

            // act
            double i = 0;
            for (i = 0; i < 10 && token==null; i+=1)
            {
                paperworkProcedure.Update(i);
                token = paperworkProcedure.GetOutputToken(0);
                paperworkProcedure.ClearOutputs();
            }

            // Asserts
            Assert.AreNotEqual(token, null);
            if (i < 1 || i > 30) Assert.Fail();
        }
    }
}
