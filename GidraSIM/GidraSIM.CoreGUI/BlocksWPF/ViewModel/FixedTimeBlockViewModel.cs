using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GidraSIM.GUI.Core.BlocksWPF;

namespace GidraSIM.GUI.Core.BlocksWPF.ViewModel
{
    public class FixedTimeBlockViewModel : ProcedureWPF
    {
        public FixedTimeBlockViewModel(Point position, string processName) : base(position, processName,1,1)
        {
        }
    }
}
