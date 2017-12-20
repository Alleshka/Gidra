using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GidraSIM.GUI.Core.BlocksWPF.ViewModel
{
    public class QualityCheckProcedureViewModel : ProcedureWPF
    {
        public QualityCheckProcedureViewModel(Point position, string processName) : base(position, processName, 1, 2)
        {
        }
    }
}
