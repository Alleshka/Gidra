using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.Serialization;

namespace GidraSIM.GUI.Core.BlocksWPF.ViewModels.Procedures
{
    [DataContract(Name = "Electr")]
    public class ElectricalSchemeSimulationViewModel : ProcedureWPF
    {
        public ElectricalSchemeSimulationViewModel(Point position, string processName) : base(position, processName, 1, 1)
        {
        }
    }
}
