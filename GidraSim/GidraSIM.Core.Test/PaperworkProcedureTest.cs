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
            PaperworkProcedure paperworkProcedure = new PaperworkProcedure();

            // act 

            // Asserts 
            Assert.AreNotEqual(paperworkProcedure, null);
        }

        [TestMethod]
        public void UpdateWithoutRes()
        {
            // arrange 
            PaperworkProcedure paperworkProcedure = new PaperworkProcedure();
            paperworkProcedure.AddToken(new Token(0, 10), 0);
            bool tes = false;

            // act 
            try
            {
                paperworkProcedure.Update(new ModelingTime() { Delta = 1, Now = 0});
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
                PaperworkProcedure paperworkProcedure = new PaperworkProcedure();
            paperworkProcedure.AddResorce(new WorkerResource()
            {
                //Name = "Alleshka",
                //Position = "Работяга",
                WorkerQualification = Qualification.FirstCategory
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
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            for (modelingTime.Now = 0; modelingTime.Now< 10 && token==null; modelingTime.Now+=modelingTime.Delta)
            {
                paperworkProcedure.Update(modelingTime);
                token = paperworkProcedure.GetOutputToken(0);
                paperworkProcedure.ClearOutputs();
            }

            // Asserts
            Assert.AreNotEqual(token, null);
            if (modelingTime.Now < 1 || modelingTime.Now > 30) Assert.Fail();
        }
    }
}
