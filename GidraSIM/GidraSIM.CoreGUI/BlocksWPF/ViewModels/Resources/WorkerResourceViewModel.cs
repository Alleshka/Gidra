using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GidraSIM.Core.Model.Resources;

namespace GidraSIM.GUI.Core.BlocksWPF.ViewModels.Resources
{
    public class WorkerResourceViewModel : ResourceWPF
    {
        public WorkerResourceViewModel(Point position, string resourceName) : base(position, resourceName)
        {
            Model = new WorkerResource();
        }

        public WorkerResource Model { get; set; }
        public string Description => Model.Description;

        Dictionary<Qualification, string> QualificationDisplay = new Dictionary<Qualification, string>
        {
            { Qualification.NoCategory,"Без категории"},
            {Qualification.FirstCategory, "Первая категория" },
            {Qualification.SecondCategory,  "Вторая категория"},
            {Qualification.ThirdCategory , "Третья категория"},
            {Qualification.LeadCategory, "Ведущий специалист" }
        };
    }
}
