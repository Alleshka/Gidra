using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GidraSIM.Core.Model.Resources;

namespace GidraSIM.GUI.Core.BlocksWPF.ViewModels.Resources
{
    public class MethodolgicalSupportResourceViewModel : ResourceWPF
    {
        public MethodolgicalSupportResourceViewModel(Point position, string resourceName) : base(position, resourceName)
        {
            Model = new MethodolgicalSupportResource();
        }
        public MethodolgicalSupportResource Model { get; set; }
        public string Description => Model.Description;
    }
}
