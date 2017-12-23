using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GidraSIM.GUI.Core.BlocksWPF.ViewModels.Procedures
{
    public class ArrangementProcedureViewModel : ProcedureWPF
    {
        public ArrangementProcedureViewModel(Point position, string processName) : base(position, processName, 1, 1)
        {
        }
    }
}
