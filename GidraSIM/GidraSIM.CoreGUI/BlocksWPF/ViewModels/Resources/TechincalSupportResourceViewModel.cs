using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GidraSIM.Core.Model.Resources;

namespace GidraSIM.GUI.Core.BlocksWPF.ViewModels.Resources
{
    public class TechincalSupportResourceViewModel : ResourceWPF
    {
        public TechincalSupportResourceViewModel(Point position, string resourceName) : base(position, resourceName)
        {
            Model = new TechincalSupportResource();
        }

        public TechincalSupportResource Model
        { get; set; }
        public string Description => Model.Description;
        public int Count => Model.Count;
    }
}
